using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public bool closeLastOneWhenOpen, shouldBeClosed;  

    public virtual void Open(){
        Debug.Log("Opening this view...");
    }
    public virtual void Close(){
        Destroy(this.gameObject);
    } 
}
