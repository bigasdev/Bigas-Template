using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ResourceController
{
    public const string iconsDirectory = "Icons/";
    public const string itemIconsDirectory = "Icons/Items";
    public const string weaponsDirectory = "/Sprites/Weapons/";
    public const string projectilesDirectory = "Projectiles";
    public const string audiosDirectory = "Audios/Sounds";
    private static Sprite[] weaponsSprites;
    private static Sprite[] itemsSprites;
    private static Sprite[] projectilesSprites;
    private static AudioClip[] soundClips;
    public static void StartSets(){
        itemsSprites = Resources.LoadAll<Sprite>(itemIconsDirectory);
        projectilesSprites = Resources.LoadAll<Sprite>(projectilesDirectory);
        soundClips = Resources.LoadAll<AudioClip>(audiosDirectory);
        foreach(var s in soundClips){
            Debug.Log(s.name);
        }
    }
    public static Sprite GetWeapon(string name){
        var stringPath = Application.streamingAssetsPath + weaponsDirectory + name + ".png";
        byte[] pngBytes = System.IO.File.ReadAllBytes(stringPath);

        Texture2D tex = new Texture2D(2,2);
        tex.filterMode = FilterMode.Point;
        tex.LoadImage(pngBytes);

        Sprite fromTex = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(.5f, .5f), 16f);
        return fromTex;
    }
    public static Sprite GetSprite(string name){
        foreach(Sprite s in itemsSprites){
            if(s.name == name){
                Debug.Log($"SUCCESS : FULLY ACCESED {name}");
                return itemsSprites.Single(s => s.name == name);
            }
        }
        Debug.Log("ERROR 001 : NO ITEM FOUND WITH THIS NAME!");
        return null;
    }
    public static Sprite GetProjectile(string name){
        foreach(Sprite s in projectilesSprites){
            if(s.name == name){
                Debug.Log($"SUCCESS : FULLY ACCESED {name}");
                return projectilesSprites.Single(s => s.name == name);
            }
        }
        Debug.Log("ERROR 001 : NO PROJECTILE FOUND WITH THIS NAME!");
        return null;
    }
    public static AudioClip GetAudio(string name){
        foreach(var a in soundClips){
            if(a.name == name){
                return a;
            }
        }
        Debug.Log("ERROR 001 : NO AUDIO FOUND WITH THIS NAME!");
        return null;
    }
}
