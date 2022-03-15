using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance{
        get{
            if(instance == null){
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    public Vector2 playerPos => new Vector2(transform.position.x, transform.position.y);
    public PlayerMovement movement;
    public Interactable lastInteractable;
    public Health health;
    public SpriteSquash squash;
    private void Start() {
        health.OnDeath += Death;
        health.OnDeath += () => health.OnDeath -= Death;
    }
    //Method to control what happens when the player dies
    void Death(){
        Time.timeScale = .3f;
        CameraManager.Instance.OnCameraZoom(1.5f, 30f);
        CameraManager.Instance.OnEndZoom += DeathDelayed;
        CameraManager.Instance.OnEndZoom += () => CameraManager.Instance.OnEndZoom -= DeathDelayed;
        StateController.Instance.ChangeState(States.GAME_DEATH);
        this.gameObject.SetActive(false);
    }
    //Method that occurs after the zoom
    void DeathDelayed(){
        //Any stuff you want to do after the death
    }
    private void Update() {
        if(StateController.Instance.currentState != States.GAME_UPDATE) return;

    }
}
