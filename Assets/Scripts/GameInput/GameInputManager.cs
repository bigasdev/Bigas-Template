using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public static class GameInputManager
{
    static bool playerIndexSet = false;
    static PlayerIndex playerIndex;
    public static GamePadState state;
    public static GamePadState prevState;
    public static void Update(){
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }
    public static bool HasJoystick(){
        return playerIndexSet;
    }
    public static float GetAxisDown(string axis){
        if(!HasJoystick()){
            if(Input.GetKey(GameInputKeys.axisMap[axis][0])){
                return 1;
            }else if(Input.GetKey(GameInputKeys.axisMap[axis][1])){
                return -1;
            }
            return 0;
        }else{
            return Input.GetAxis(axis);
        }
    }
    public static bool GetKeyDown(string keyMap){
        if(!HasJoystick()){
            return Input.GetKeyDown(GameInputKeys.keyMapping[keyMap]);
        }
        return GameInputKeys.GetGamePadButton(keyMap);
    }
    public static bool GetRightClick(){
        return Input.GetMouseButtonDown(1);
    }
    public static bool GetLeftClick(){
        if(!HasJoystick()){
            return Input.GetMouseButtonDown(0);
        }
        return prevState.Triggers.Right > 0;
    }
}
public static class GameInputKeys{
    public static Dictionary<string, KeyCode[]> axisMap;
    public static Dictionary<string, KeyCode> keyMapping;
    public static Dictionary<string, ButtonState> gamepadMapping;
    public static void CreateDictionarys(){
        axisMap = new Dictionary<string, KeyCode[]>();
        axisMap.Add("Horizontal", Horizontal);
        axisMap.Add("Vertical", Vertical);
        keyMapping = new Dictionary<string, KeyCode>();
        gamepadMapping = new Dictionary<string, ButtonState>();
        for(int i = 0; i < keyMaps.Length; i++){
            keyMapping.Add(keyMaps[i], defaults[i]);
            gamepadMapping.Add(keyMaps[i], gamepadDefaults[i]);
        }
    }
    public static void CreateGamepad(){
        gamepadMapping = new Dictionary<string, ButtonState>();
        for(int i = 0; i < keyMaps.Length; i++){
            gamepadMapping.Add(keyMaps[i], gamepadDefaults[i]);
        }
    }
    private static KeyCode[] Horizontal = {
        KeyCode.D,
        KeyCode.A
    };
    private static KeyCode[] Vertical = {
        KeyCode.W,
        KeyCode.S
    };
    static string[] keyMaps = new string[4]{
        "Inventory",
        "Pause",
        "Interaction",
        "Use",
    };
    static KeyCode[] defaults = new KeyCode[4]{
        KeyCode.I,
        KeyCode.Escape,
        KeyCode.E,
        KeyCode.Q,
    };
    static bool[] gamepadDefaultsd = new bool[4]{
        GameInputManager.prevState.Buttons.Back == ButtonState.Pressed,
        GameInputManager.prevState.Buttons.Start == ButtonState.Pressed,
        GameInputManager.prevState.Buttons.A == ButtonState.Pressed,
        GameInputManager.prevState.Buttons.Y == ButtonState.Pressed
    };
    static ButtonState[] gamepadDefaults = new ButtonState[4]{
        GameInputManager.state.Buttons.Back,
        GameInputManager.state.Buttons.Start,
        GameInputManager.state.Buttons.A,
        GameInputManager.state.Buttons.Y
    };
    public static ButtonState ReturnPrevButtonState(string name){
        if(name == "Inventory"){
            return GameInputManager.prevState.Buttons.Back;
        }else if(name == "Pause"){
            return GameInputManager.prevState.Buttons.Start;
        }else if(name == "Interaction"){
            return GameInputManager.prevState.Buttons.Y;
        }
        return ButtonState.Released;
    }
    public static ButtonState ReturnButtonState(string name){
        if(name == "Inventory"){
            return GameInputManager.state.Buttons.Back;
        }else if(name == "Pause"){
            return GameInputManager.state.Buttons.Start;
        }else if(name == "Interaction"){
            return GameInputManager.state.Buttons.Y;
        }
        return ButtonState.Released;
    }
    public static bool GetGamePadButton(string name){
        return ReturnPrevButtonState(name) == ButtonState.Pressed && ReturnButtonState(name) == ButtonState.Released;
    }
}
