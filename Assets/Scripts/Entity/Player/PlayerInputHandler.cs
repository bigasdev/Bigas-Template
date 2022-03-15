using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject console;
    View view;
    private void Update() {
        GameInputManager.Update();
        if(GameInputManager.GetKeyDown("Pause")){
            if(Engine.Views.CloseLastOpen()){
                return;
            }
            OpenPause();
        }
        if(Input.GetKeyDown(KeyCode.Comma) && StateController.Instance.currentState == States.GAME_UPDATE){
            Instantiate(console);
        }
    }
    bool TryToCloseStuff(){
        if(Engine.Views.CloseLastOpen()){
            return true;
        }
        return false;
    }
    void OpenPause(){
        //Open pause menu
    }
}
