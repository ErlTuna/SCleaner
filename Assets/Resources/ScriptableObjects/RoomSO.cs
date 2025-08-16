using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomSO", menuName = "ScriptableObjects/Room Info")]
public class RoomSO : ScriptableObject
{
    public List<GameObject> enemyList;
    public int roomID;
    public Vector3 roomLocation;
    //ensure this collider is in trigger mode
    public BoxCollider2D roomArea = new();
    public int enemyCount;
    //enemy type is needed

}
