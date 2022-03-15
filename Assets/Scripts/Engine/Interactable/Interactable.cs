using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractableType type;
    InteractablePopup popup;
    public virtual void OnSelect(){
        if(popup == null){
            //Spawn the popup
        }
    }
    public virtual void OnInteract(){

    }
    public virtual void OnDeselect(){
        if(popup != null)
        popup.anim.SetTrigger("Leave");
    }
}
[System.Serializable]
public class InteractableType{
    public string interactionName;

    public InteractableType(string interactionName)
    {
        this.interactionName = interactionName;
    }
}
