using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen instance;
    public static LoadingScreen Instance{
        get{
            if(instance == null){
                instance = FindObjectOfType<LoadingScreen>();
            }
            return instance;
        }
    }
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject player;
    public void StartLoad(LoadSettings settings){
        StartCoroutine(LoadingScreenMethod(settings));
    }
    IEnumerator LoadingScreenMethod(LoadSettings scene){
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.sceneName);
        while (!operation.isDone){
            yield return null;
        }
        yield return new WaitForSeconds(.25f);
        GameObject p = null;
        if(scene.loadCharacter)p = Instantiate(player);
        var s = FindObjectOfType<PlayerSpawnPoint>();
        if(s != null && p != null)p.transform.position = s.transform.position;
        loadingScreen.SetActive(false);
    }
}
[System.Serializable]
public class LoadSettings{
    public string sceneName;
    public bool loadCharacter;
    public bool loadMap;
    public string mapName = "";

    public LoadSettings(string name, bool ch = true, bool mp = true, string mpName = "")
    {
        sceneName = name;
        loadCharacter = ch;
        loadMap = mp;
        mapName = mpName;
    }
}