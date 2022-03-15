using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InteractablePopup : MonoBehaviour
{
    [SerializeField] Text interactionName;
    public Animator anim;
    public void Initialize(InteractableType type){
        interactionName.text = type.interactionName;
    }
}
