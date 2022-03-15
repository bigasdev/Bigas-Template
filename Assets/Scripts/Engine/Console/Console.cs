using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Console : MonoBehaviour
{
    [SerializeField] private InputField text;
    [SerializeField] private Text commandsText;
    private EventSystem system;
    bool isDev, isQuit;
    private void Start() {
        Initialize();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Comma)){
            StateController.Instance.ChangeState(States.GAME_UPDATE);
            Time.timeScale = 1;
            Destroy(this.gameObject);
        }
    }
    void TryParseCommand(string _text){
        text.text = "";
        string command = "";
        commandsText.text += _text + "\n";
        foreach(var c in _text){
            command += c;
            if(LookForComands(command)){
                command = null;
            }
        }
    }
    bool LookForComands(string _text){
        if(_text == "/dev"){
            Debug.Log($"VALID COMMAND!");
            Engine.isDevMode = !Engine.isDevMode;
            isDev = true;
            return true;
        }
        if(_text == "/quit"){
            Debug.Log($"VALID COMMAND!");
            isQuit = true;
            Application.Quit();
            return true;
        }
        if(_text == "/reset"){
            DataController.Instance.ResetData();
            return true;
        }
        return false;
    }
    void Initialize(){
        system = FindObjectOfType<EventSystem>();
        system.SetSelectedGameObject(text.gameObject);
        StateController.Instance.ChangeState(States.GAME_CONSOLE);
        Time.timeScale = 0;
        text.onEndEdit.AddListener(delegate{TryParseCommand(text.text);});
    }
}
