using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRandomItem : MonoBehaviour
{
    
    public List<GameObject> lootTable;
    public Transform itemHolder;

    void Awake(){
        
    }

    int PickRandomItem(){
        //hacky implementation...
        return Random.Range(0, 11);
    }

    public void DropItem(){
        
        
        if(lootTable.Count == 0){
            print("NO ITEM IN LOOT TABLE");
            return;
        }

        int rand = PickRandomItem();
        GameObject item = null;
        if(rand < lootTable.Count){
            item = lootTable[rand];
        }
        
        if(item != null){
        Vector3 location = itemHolder.position;
        Instantiate(item, location, Quaternion.identity);
        }    
    }
}
