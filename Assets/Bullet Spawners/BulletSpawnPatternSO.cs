using System.Collections;
using UnityEngine;

public abstract class BulletSpawnPatternSO : ScriptableObject
{
    public abstract IEnumerator Execute(BulletSpawner spawner, GameObject owner);

}
