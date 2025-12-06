using UnityEngine;

[CreateAssetMenu(fileName = "BulletInfoSO", menuName = "ScriptableObjects/Bullet Info")]
public class BulletConfigSO : ScriptableObject
{
    public GameObject Prefab;
    public int DefaultDamage = 1;
    public float DefaultProjectileSpeed = 5f;
    public float DefaultSize = 1f;
    public float DefaultLifeTime = 5f;
    public float DefaultPushForce = 0f;



    //public BulletType Type;

}

/*
public enum BulletType
{
    PROJECTILE,
    HITSCAN
}
*/