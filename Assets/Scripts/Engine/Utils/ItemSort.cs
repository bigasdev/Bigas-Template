using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSort
{
    public SpriteRenderer spriteRenderer;
    private const int behindPlayer = -1;
    private const int abovePlayer = 1;
    int currentOrder = 0;
    Transform _ref;
    public ItemSort(SpriteRenderer sprite, Transform tRef){
        spriteRenderer = sprite;
        _ref = tRef;
    }
    void SortLayer(){
        spriteRenderer.sortingOrder = currentOrder;
    }
    public void UpdateSort(){
        if(Player.Instance == null) return;
        if(Vector2.Distance(Player.Instance.transform.position, _ref.position) > 1) return;
        int order = Player.Instance.transform.position.y >= _ref.transform.position.y ? abovePlayer : behindPlayer;
        if(order != currentOrder){
            currentOrder = order;
            SortLayer();
        }
    }
}
