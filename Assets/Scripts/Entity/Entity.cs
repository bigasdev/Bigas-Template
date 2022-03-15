using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Components")]
    public string entityName;
    public Animator anim;
    public SpriteRenderer sprite;
    public BoxCollider2D col;
    public Rigidbody2D rb;
    public Health health;
    ItemSort sort;
    public Vector2 hitPos;
    [Header("Parameters")]
    public float hitBlinkTime = .1f;
    public float knockbackForce = 2f;
    public SpriteSquash spriteSquash;
    public virtual Vector2 Position{
        get{
            return this.transform.position;
        }
        set{
            this.transform.position = value;
        }
    }
    public virtual void Start() {
        CreateComponents();
        sort = new ItemSort(sprite, this.transform);
    }
    public virtual void CreateComponents(){
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteSquash = this.gameObject.AddComponent<SpriteSquash>();
        spriteSquash.playerSprite = sprite.transform;
        health.OnDeath += Death;
    }
    public virtual void Death(){
        CameraManager.KillShake();
    }
    public virtual void Update(){
        //sort.UpdateSort();
    }
}

