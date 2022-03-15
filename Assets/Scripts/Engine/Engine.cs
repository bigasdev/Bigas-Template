using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public static string language = "EN";
    private static List<EngineComponent> _components;
    public static bool isDevMode = false;
    public static EngineMouse Mouse{
        get{
            return GetEngineComponent<EngineMouse>();
        }
    }
    public static EngineViews Views{
        get{
            return GetEngineComponent<EngineViews>();
        }
    }

    private void Start() {
        CreateComponents();
        GameInputKeys.CreateDictionarys();
        Application.targetFrameRate = 144;
        foreach(var c in _components){
            c.Initialize();
        }
    }
    void CreateComponents(){
       EngineComponent[] c = FindObjectsOfType<EngineComponent>();
       _components = new List<EngineComponent>(c);
    }
    public static T GetEngineComponent<T>() where T : EngineComponent{
        return _components.Find(x => x is T) as T;
    }
    private void Update() {
        Mouse.Update();
    }
}
public static class EngineCommands{
    public static Vector2Int ToInt2(Vector2 v)
    {
        return new Vector2Int((int)v.x, (int)v.y);
    }
}