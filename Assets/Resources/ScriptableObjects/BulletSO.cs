using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BulletInfoSO", menuName = "ScriptableObjects/Bullet Info")]
public class BulletSO : ScriptableObject
{
    public BulletType bulletType;
    public float projectileSpeed = 5f;
    public float size = 1f;
    public float lifeTime = 5f;

    public enum BulletType{
    PROJECTILE,
    HITSCAN
}
}


