using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataController : MonoBehaviour
{
    private static DataController instance;
    public static DataController Instance{
        get{
            if(instance == null){
                instance = FindObjectOfType<DataController>();
            }
            return instance;
        }
    }
    public string player_data = "1";
    public string slot1 = "player_data_01", slot2 = "player_data_02", slot3 = "player_data_03", slot4 = "player_data_04";
    public PlayerData playerData;
    float playTime;
    public bool hasFirstSlot => PlayerPrefs.HasKey(slot1);
    public bool hasSecondSlot => PlayerPrefs.HasKey(slot2);
    public bool hasThirdSlot => PlayerPrefs.HasKey(slot3);
    void Awake()
    {
        var othercontroller = FindObjectsOfType<DataController>();
        if(othercontroller.Length > 1) Destroy(othercontroller[1].gameObject);
        ResourceController.StartSets();
        DontDestroyOnLoad(this);
    }
    public void Load(string fileName) {
        string playerFile = string.Empty;

        if (!PlayerPrefs.HasKey(fileName)) {
            Debug.Log("Created new save: " + fileName);
            playerFile = JsonUtility.ToJson(new PlayerData());
            PlayerPrefs.SetString(fileName, playerFile);
        }


        playerFile = PlayerPrefs.GetString(fileName);
        playerData = JsonUtility.FromJson<PlayerData>(playerFile);
        CreateLog(JsonUtility.FromJson<string>(playerFile));
        player_data = fileName;
    }
    public float GetCurrentTime(string fileName){
        var file = PlayerPrefs.GetString(fileName);
        var data = JsonUtility.FromJson<PlayerData>(file);
        return data.playTime;
    }

    public void Save() {
        string playerFile = JsonUtility.ToJson(playerData);
        playerData.playTime += playerData.playTime + playTime;
        PlayerPrefs.SetString(player_data, playerFile);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log($"Game Saved.");
    }
    private void Update() {
        playTime += Time.deltaTime;
    }
    public void CreateLog(string args){
        string path = Application.dataPath + "/Log.txt";

        if(!File.Exists(path)){
            File.WriteAllText(path, "Data log \n\n");
        }
        File.AppendAllText(path, args);  
    }
    public void ResetData() {
        PlayerPrefs.DeleteAll();

        //Save();
    }
}
