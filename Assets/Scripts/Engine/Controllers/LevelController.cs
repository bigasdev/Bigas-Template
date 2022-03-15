using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private static LevelController instance;
    public static LevelController Instance{
        get{
            if(instance == null){
                instance = FindObjectOfType<LevelController>();
            }
            return instance;
        }
    }
    public GameObject spawn(GameObject prefab, Vector2 pos){
        return Instantiate(prefab, pos, Quaternion.identity);
    }
    public static GameObject Spawn(GameObject prefab, Vector2 pos){
        return LevelController.Spawn(prefab, pos);
    }
}
