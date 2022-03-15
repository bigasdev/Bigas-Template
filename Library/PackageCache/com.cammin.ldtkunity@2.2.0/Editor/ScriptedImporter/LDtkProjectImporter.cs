﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using Object = UnityEngine.Object;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

#pragma warning disable 0414
#pragma warning disable 0649

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_PROJECT)]
    [ScriptedImporter(LDtkImporterConsts.PROJECT_VERSION, LDtkImporterConsts.PROJECT_EXT, LDtkImporterConsts.PROJECT_ORDER)]
    internal class LDtkProjectImporter : LDtkJsonImporter<LDtkProjectFile>
    {
        public const string JSON = nameof(_jsonFile);

        public const string PIXELS_PER_UNIT = nameof(_pixelsPerUnit);
        public const string ATLAS = nameof(_atlas);
        public const string CUSTOM_LEVEL_PREFAB = nameof(_customLevelPrefab);
        public const string DEPARENT_IN_RUNTIME = nameof(_deparentInRuntime);
        public const string INTGRID_VISIBLE = nameof(_intGridValueColorsVisible);
        public const string USE_COMPOSITE_COLLIDER = nameof(_useCompositeCollider);
        public const string CREATE_BACKGROUND_COLOR = nameof(_createBackgroundColor);
        
        public const string INTGRID = nameof(_intGridValues);
        public const string ENTITIES = nameof(_entities);
        
        public const string ENUM_GENERATE = nameof(_enumGenerate);
        public const string ENUM_PATH = nameof(_enumPath);
        public const string ENUM_NAMESPACE = nameof(_enumNamespace);
        
        
        /// <summary>
        /// This is cached into the meta file upon an import. Could be null if the import was a failure. Invisible to the inspector.
        /// </summary>
        [SerializeField] private LDtkProjectFile _jsonFile;

        [SerializeField] private int _pixelsPerUnit = -1;
        [SerializeField] private SpriteAtlas _atlas;
        [SerializeField] private GameObject _customLevelPrefab = null;
        [SerializeField] private bool _deparentInRuntime = false;
        [SerializeField] private bool _intGridValueColorsVisible = false;
        [SerializeField] private bool _useCompositeCollider = true;
        [SerializeField] private bool _createBackgroundColor = true;
        
        [SerializeField] private LDtkAssetIntGridValue[] _intGridValues = Array.Empty<LDtkAssetIntGridValue>();
        
        [SerializeField] private LDtkAssetEntity[] _entities = Array.Empty<LDtkAssetEntity>();
        
        [SerializeField] private bool _enumGenerate = false;
        [SerializeField] private string _enumPath = null;
        [SerializeField] private string _enumNamespace = string.Empty;

        
        public LDtkProjectFile JsonFile => _jsonFile;
        public bool IntGridValueColorsVisible => _intGridValueColorsVisible;
        public SpriteAtlas Atlas => _atlas;
        public int PixelsPerUnit => _pixelsPerUnit;
        public bool DeparentInRuntime => _deparentInRuntime;
        public GameObject CustomLevelPrefab => _customLevelPrefab;
        public bool UseCompositeCollider => _useCompositeCollider;
        public bool CreateBackgroundColor => _createBackgroundColor;
        public string AssetName => Path.GetFileNameWithoutExtension(assetPath);
        
        private LDtkArtifactAssets _artifacts;
        private bool _hadTextureProblem;
        
        //this will run upon standard reset, but also upon the meta file generation during the first import
        private void Reset()
        {
            OnResetPPU();
        }

        protected override void Import()
        {
            _hadTextureProblem = false;
            
            CreateJsonAsset();

            if (!TryGetJson(out LdtkJson json))
            {
                Debug.LogError("LDtk: Json deserialization error. Not importing.");
                return;
            }

            //if for whatever reason (or backwards compatibility), if the ppu is -1 in any capacity
            SetPixelsPerUnit((int) json.DefaultGridSize);
            
            CreateArtifactAsset();

            MainBuild(json);
            
            SetupAllAssetDependencies();
            TryGenerateEnums(json);
            HideArtifactAssets();
            TryPrepareSpritePacking(json);
            BufferEditorCache();

            if (EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D)
            {
                Debug.LogWarning("LDtk: It is encouraged to use 2D project mode while using LDtkToUnity. Change it in \"Project Settings > Editor > Default Behaviour Mode\"");
            }
        }

        private void SetupAllAssetDependencies()
        {
            //trigger a reimport if any of these involved assets are saved or otherwise changed in source control
            SetupAssetDependencies(_intGridValues.Distinct().Cast<ILDtkAsset>().ToArray());
            SetupAssetDependencies(_entities.Distinct().Cast<ILDtkAsset>().ToArray());

            if (_customLevelPrefab != null)
            {
                SetupAssetDependency(_customLevelPrefab);
            }
        }

        private void TryPrepareSpritePacking(LdtkJson json)
        {
            //allow the sprites to be gettable in the AssetDatabase properly; only after the import process
            if (_hadTextureProblem)
            {
                Debug.LogWarning("LDtk: Did not pack tile textures, a previous tile error was encountered.");
                return;
            }
            
            if (_atlas == null || !LDtkProjectImporterAtlasPacker.UsesSpriteAtlas(json))
            {
                return;
            }

            EditorApplication.delayCall += () =>
            {
                LDtkProjectImporterAtlasPacker packer = new LDtkProjectImporterAtlasPacker(_atlas, assetPath);
                packer.TryPack();
            };
        }

        private void HideArtifactAssets()
        {
            //need to keep the sprites visible in the project view if using sprite atlas
            if (_atlas == null)
            {
                _artifacts.HideSprites();
            }

            _artifacts.HideTiles();
            
            _artifacts.HideBackgrounds();
        }

        private bool TryGetJson(out LdtkJson json)
        {
            json = _jsonFile.FromJson;
            if (json != null)
            {
                return true;
            }
            
            ImportContext.LogImportError("LDtk: Json import error");
            return false;

        }

        private void CreateJsonAsset()
        {
            _jsonFile = ReadAssetText();
            _jsonFile.name += "_Json";
            ImportContext.AddObjectToAsset("jsonFile", _jsonFile, (Texture2D)LDtkIconUtility.LoadWorldIcon());
        }
        
        private void BufferEditorCache()
        {
            EditorApplication.delayCall += () =>
            {
                LDtkJsonEditorCache.ForceRefreshJson(assetPath);
            };
        }

        private void MainBuild(LdtkJson json)
        {
            LDtkProjectImporterFactory factory = new LDtkProjectImporterFactory(this);
            factory.Import(json);
        }

        private void CreateArtifactAsset()
        {
            //the bank for storing the auto-generated items.
            _artifacts = ScriptableObject.CreateInstance<LDtkArtifactAssets>();
            _artifacts.name = AssetName + "_Assets";
            
            ImportContext.AddObjectToAsset("artifacts", _artifacts, (Texture2D)LDtkIconUtility.GetUnityIcon("Tilemap"));
        }

        private void TryGenerateEnums(LdtkJson json)
        {
            //generate enums
            if (!_enumGenerate || json.Defs.Enums.IsNullOrEmpty())
            {
                return;
            }
            
            LDtkProjectImporterEnumGenerator enumGenerator = new LDtkProjectImporterEnumGenerator(json.Defs.Enums, ImportContext, _enumPath, _enumNamespace);
            enumGenerator.Generate();
        }

        public void AddArtifact(Object obj)
        {
            if (_artifacts.AddArtifact(obj))
            {
                ImportContext.AddObjectToAsset(obj.name, obj);
            }
        }

        public void AddBackgroundArtifact(Sprite obj)
        {
            _artifacts.AddBackground(obj);
            ImportContext.AddObjectToAsset(obj.name, obj);
        }

        private void SetupAssetDependencies(ILDtkAsset[] assets)
        {
            //dependencies. reimport if any of these assets change
            if (assets.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (ILDtkAsset asset in assets)
            {
                if (asset.Asset == null)
                {
                    continue;
                }

                SetupAssetDependency(asset.Asset);
            }
        }

        public LDtkIntGridTile GetIntGridValueTile(string key)
        {
            return GetAssetByIdentifier(_intGridValues, key);
        }
        public GameObject GetEntity(string key)
        {
            return GetAssetByIdentifier(_entities, key);
        }

        private T GetAssetByIdentifier<T>(IEnumerable<LDtkAsset<T>> input, string key) where T : Object
        {
            if (input == null)
            {
                ImportContext.LogImportError("LDtk: Tried getting an asset from the build data but the array was null. Is the project asset properly saved?");
                return default;
            }

            foreach (LDtkAsset<T> asset in input)
            {
                if (ReferenceEquals(asset, null))
                {
                    ImportContext.LogImportError($"LDtk: A field in the build data is null.");
                    continue;
                }

                if (asset.Key != key)
                {
                    continue;
                }

                if (asset.Asset == null)
                {
                    continue;
                }
                
                return (T)asset.Asset;
            }
            
            return default;
        }

        public TileBase GetTile(Texture2D srcTex, Vector2Int srcPos, int pixelsPerUnit)
        {
            LDtkTileArtifactFactory creator = new LDtkTileArtifactFactory(this, _artifacts, srcTex, srcPos, pixelsPerUnit);
            TileBase tile = creator.TryGetOrCreateTile();
            if (tile == null)
            {
                ImportContext.LogImportError("LDtk: Tried retrieving a Tile from the importer's assets, but was null.");
                _hadTextureProblem = true;
            }

            return tile;
        }
        
        private void OnResetPPU()
        {
            if (_pixelsPerUnit > 0)
            {
                return;
            }

            //deserializing json is time costly, so only do it when we necessarily must
            LdtkJson json = ReadJson();
            if (json == null)
            {
                //if json problem, then default to what LDtk also defaults to upon a new project
                _pixelsPerUnit = LDtkImporterConsts.DEFAULT_PPU;
                return;
            }
            SetPixelsPerUnit((int) json.DefaultGridSize);
        }

        private void SetPixelsPerUnit(int ppu)
        {
            if (_pixelsPerUnit > 0)
            {
                return;
            }
            
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty ppuProp = serializedObject.FindProperty(PIXELS_PER_UNIT);
            ppuProp.intValue = ppu;
            serializedObject.ApplyModifiedProperties();
        }
        
        public IEnumerable<TileBase> GetIntGridTiles()
        {
            return _intGridValues.Select(p => p.Asset).Cast<TileBase>().ToArray();
        }
    }
}
