using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[CreateAssetMenu(fileName = "BulletInfoSO", menuName = "ScriptableObjects/Bullet Info")]
public class BulletConfigSO : ScriptableObject
{
    public GameObject Prefab;
    public BulletType Type;
    public int Damage = 1;
    public float ProjectileSpeed = 5f;
    public float Size = 1f;
    public float LifeTime = 5f;




}
public enum BulletType
{
    PROJECTILE,
    HITSCAN
}