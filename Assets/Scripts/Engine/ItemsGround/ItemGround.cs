using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemGround : Interactable
{
    public Item item;
    public SpriteRenderer itemSprite;
    public Transform layerRef;
    [SerializeField] float flySpeed;
    ItemSort layerSort;
    Coroutine sequence, fly;
    Rigidbody2D rb;

    public Vector2 Velocity{
        get{return rb.velocity;}
        set{rb.velocity = value;}
    }
    public virtual void Initialize(Item container){
        item = container;
        itemSprite.sprite = ResourceController.GetSprite(container.iconSpriteName);
    }
    public virtual void Awake(){
        layerSort = new ItemSort(itemSprite, layerRef);
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void Update(){
        layerSort.UpdateSort();
        if(Vector2.Distance(Player.Instance.transform.position, this.transform.position) <= 1.6f){
            OnInteract();
        }
        if(sequence == null) sequence = StartCoroutine(FloatAction());
    }
    public void Move(Vector2 velocity){
        StartCoroutine(MoveAction());
        Velocity = velocity;
    }
    IEnumerator FloatAction(){
        itemSprite.GetComponent<Transform>().DOLocalMoveY(.15f, .65f);
        yield return new WaitForSeconds(.65f);
        itemSprite.GetComponent<Transform>().DOLocalMoveY(0f, .65f);
        yield return new WaitForSeconds(.65f);
        sequence = null;
    }
    IEnumerator MoveAction(){
        rb.isKinematic = false;
        yield return new WaitForSeconds(.15f);
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }
    public override void OnSelect()
    {
        //base.OnSelect();
    }
    public override void OnInteract()
    {
        //base.OnInteract();
        if(fly == null) fly = StartCoroutine(FlyTo());
    }
    IEnumerator FlyTo(){
        Vector2 pos = Player.Instance.transform.position;
        while(Vector2.Distance(this.transform.position, pos) >= .05f){
            this.transform.position = Vector2.MoveTowards(this.transform.position, pos, flySpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }
    public override void OnDeselect()
    {
        //base.OnDeselect();
    }
}
