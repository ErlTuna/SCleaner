using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Simple Bullet Pattern", menuName = "ScriptableObjects/Bullet Patterns/Simple Pattern")]
public class SimpleBulletPattern : BulletSpawnPatternSO
{
    [SerializeField] int bulletCount;
    [SerializeField] float timeBetweenShots;
    [SerializeField] WaitForSeconds fireDelay;
    [SerializeField] float rotationOffset;
    [SerializeField] float angleStep;

    public override IEnumerator Execute(BulletSpawner spawner, GameObject owner)
    {        
        fireDelay ??= new WaitForSeconds(timeBetweenShots);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = rotationOffset + (i * angleStep);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

            Bullet bullet = spawner.GetBullet();
            bullet.Initialize(owner, direction);
            bullet.transform.position = spawner.transform.position;
            //bullet.transform.right = direction;

            if(timeBetweenShots > 0)
                yield return fireDelay;
        }
    }
}
