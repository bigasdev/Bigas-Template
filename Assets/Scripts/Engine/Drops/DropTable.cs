using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] ItemContainer[] commonData;
    [SerializeField] ItemContainer[] uncommonData;
    [SerializeField] ItemContainer[] rareData;
    [SerializeField] ItemContainer[] veryRareData;
    [SerializeField] ItemContainer[] uniqueData;
    [SerializeField] ItemContainer selectedData;
    [Header("Percentages")]
    [SerializeField] int commonPercentage;
    [SerializeField] int uncommonPercentage;
    [SerializeField] int rarePercentage;
    [SerializeField] int veryRarePercentage;
    [SerializeField] int uniquePercentage;
    int[] totalPercentage;
    int lastPercentage;

    [Header("GameObjects")]
    [SerializeField] GameObject item;
    public virtual void Start() {
        CreatePercentages();
    }
    public virtual void OnDrop(Vector2 pos){
        GetDrop();
        Debug.Log("Dropping item : " + selectedData.itemName + " With : " + (lastPercentage - 100) + "%");
        var i = Instantiate(item, pos, Quaternion.identity);
        //i.GetComponent<ItemGround>().Initialize(selectedData);
    }
    void CreatePercentages(){
        totalPercentage = new int[4];
        totalPercentage[0] = commonPercentage;
        totalPercentage[1] = uncommonPercentage;
        totalPercentage[2] = rarePercentage;
        totalPercentage[3] = veryRarePercentage;
    }
    int GetDrop(){
        var range = 0;
        for(int i = 0; i < totalPercentage.Length; i++){
            range += totalPercentage[i];
        }
        var rand = Random.Range(0, range);
        var top = 0;
        for(int i = 0; i < totalPercentage.Length; i++){
            top += totalPercentage[i];
            if(rand < top){
                if(i == 0){
                    selectedData = GetData(commonData, Random.Range(0, commonData.Length - 1));
                }else if(i == 1){
                    selectedData = GetData(uncommonData, Random.Range(0, uncommonData.Length - 1));
                }else if(i == 2){
                    selectedData = GetData(rareData, Random.Range(0, rareData.Length - 1));
                }else if(i == 3){
                    selectedData = GetData(veryRareData, Random.Range(0, veryRareData.Length - 1));
                }
                lastPercentage = rand;
                return i;
            }
        }
        return 0;
    }
    ItemContainer GetData(ItemContainer[] container, int number){
        return container[number];
    }
}
