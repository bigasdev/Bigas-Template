using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineMouse : EngineComponent
{
    public Vector2 MousePos{
        get{
            if(Camera.main == null) return new Vector2(0,0);
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            return worldPos;
        }
    }
    public void Update() {

    }

}
