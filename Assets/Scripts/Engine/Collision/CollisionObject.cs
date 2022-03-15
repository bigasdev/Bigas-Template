using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionObject : MonoBehaviour
{
    private Sprite sprite;
    [SerializeField]Tilemap tilemap;
    [SerializeField]float offsetY = .6f;
    // Start is called before the first frame update
    void Start(){
        tilemap = GetComponent<Tilemap>();
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace))
            {
                var o =Instantiate(Resources.Load<GameObject>("Prefabs/Map/Collisions/CollisionBlock"), this.transform.parent);
                o.transform.position = pos;
            }
        }
        this.transform.parent.position = new Vector2(this.transform.parent.position.x, this.transform.parent.position.y + offsetY);
        GetComponent<TilemapRenderer>().enabled = false;
    }
}
