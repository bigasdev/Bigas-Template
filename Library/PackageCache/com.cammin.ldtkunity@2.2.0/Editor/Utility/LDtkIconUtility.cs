﻿using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkIconUtility
    {
        private const string PATH_ROOT = "Icons/";
        private const string PATH_LIGHT = PATH_ROOT + "Light/";
        private const string PATH_DARK = PATH_ROOT + "Dark/";
        private const string PATH_MISC = PATH_ROOT + "Misc/";
        
        
        private const string AUTO_LAYER = "AutoLayer";
        private const string ENTITY = "Entity";
        private const string ENUM = "Enum";
        private const string INT_GRID = "IntGrid";
        private const string LAYER = "Layer";
        private const string LEVEL = "Level";
        private const string LIST = "List";
        private const string TILESET = "Tileset";
        private const string WORLD = "World";
        
        
        private const string FAV = "Fav";
        private const string LEVEL_FILE = "LevelFile";
        private const string PROJECT_FILE = "ProjectFile";
        private const string SIMPLE = "Simple";
        
        private const string SQUARE = "Square";
        private const string CIRCLE = "Circle";
        private const string CROSS = "Cross";
        
        
        //private static readonly Dictionary<string, Texture2D> CachedIcons = new Dictionary<string, Texture2D>();
        


        public static Texture2D LoadAutoLayerIcon() => LoadIcon(AUTO_LAYER, true);
        public static Texture2D LoadEntityIcon() => LoadIcon(ENTITY, true);
        public static Texture2D LoadEnumIcon() => LoadIcon(ENUM, true);
        public static Texture2D LoadIntGridIcon() => LoadIcon(INT_GRID, true);
        public static Texture2D LoadLayerIcon() => LoadIcon(LAYER, true);
        public static Texture2D LoadLevelIcon() => LoadIcon(LEVEL, true);
        public static Texture2D LoadListIcon() => LoadIcon(LIST, true);
        public static Texture2D LoadTilesetIcon() => LoadIcon(TILESET, true);
        public static Texture2D LoadWorldIcon() => LoadIcon(WORLD, true);
        
        
        public static Texture2D LoadFavIcon() => LoadIcon(FAV);
        public static Texture2D LoadLevelFileIcon() => LoadIcon(LEVEL_FILE);
        public static Texture2D LoadProjectFileIcon() => LoadIcon(PROJECT_FILE);
        public static Texture2D LoadSimpleIcon() => LoadIcon(SIMPLE);
        public static Texture2D LoadSquareIcon() => LoadIcon(SQUARE);
        public static Texture2D LoadCircleIcon() => LoadIcon(CIRCLE);
        public static Texture2D LoadCrossIcon() => LoadIcon(CROSS);
        
        

        private static Texture2D LoadIcon(string fileName, bool lightThemeSkinPossible = false)
        {
            /*if (CachedIcons.ContainsKey(fileName))
            {
                return CachedIcons[fileName];
            }*/

            string directory = GetLoadPath(lightThemeSkinPossible);
            string path = $"{directory}{fileName}.png";
            Texture2D tex = LDtkInternalUtility.Load<Texture2D>(path);

            /*if (tex != null)
            {
                CachedIcons.Add(fileName, tex);
            }*/

            return tex;
        }

        private static string GetLoadPath(bool lightThemeSkinPossible)
        {
            if (lightThemeSkinPossible)
            {
                return EditorGUIUtility.isProSkin ? PATH_DARK : PATH_LIGHT;
            }

            return PATH_MISC;
        }

        public static Texture GetUnityIcon(string name, string ending = " Icon")
        {
            string tilemapIcon = EditorGUIUtility.isProSkin ? $"d_{name}{ending}" : $"{name}{ending}";
            return EditorGUIUtility.IconContent(tilemapIcon).image;
        }
        
        public static Texture2D GetIconForLayerInstance(LayerInstance instance)
        {
            if (instance.IsIntGridLayer)
            {
                return LoadIntGridIcon();
            }
            if (instance.IsEntitiesLayer)
            {
                return LoadEntityIcon();
            }
            if (instance.IsTilesLayer)
            {
                return LoadTilesetIcon();
            }
            if (instance.IsAutoLayer)
            {
                return LoadAutoLayerIcon();
            }

            return null;
        }

        public static Texture2D GetRenderStaticPreview(Texture2D icon, int width, int height)
        {
            if (icon == null)
            {
                return null;
            }

            //from example https://docs.unity3d.com/ScriptReference/Editor.RenderStaticPreview.html
            // example.PreviewIcon must be a supported format: ARGB32, RGBA32, RGB24, Alpha8 or one of float formats
            
            Texture2D tex = new Texture2D(width, height);
            EditorUtility.CopySerialized(icon, tex);
            return tex;
        }
    }
}