using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class CurrencyDropper : MonoBehaviour
{
    public Transform itemHolder;
    IDefeatable _defeatable;

    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] LootConfigSO _lootConfigSO;
    [SerializeField] int _angleStep;
    Transform _player;

    void OnDisable()
    {
        //_defeatable.OnDefeat -= Drop;
    }


    public void Initialize(IDefeatable defeatable, Transform player)
    {
        _defeatable = defeatable;
        _player = player;
        _defeatable.OnDefeat += Drop;
    }

    void Drop()
    {
        Debug.Log("ATTEMPTING ITEM DROP");
        int totalWeight = CalculateWeight();
        LootEntry entry = PickRandomLootFromTable(totalWeight);
        int count = Random.Range(entry.MinAmount, entry.MaxAmount + 1);

        if(entry.ItemSO)
        {
            //Debug.Log("DROPPING ITEM");
            //Vector3 location = itemHolder.position;
            //Instantiate(entry.ItemSO.Prefab, location, Quaternion.identity);
            StartCoroutine(DropItemInDirections(entry.ItemSO.Prefab, count));
        }
    }

    int CalculateWeight()
    {
        int totalWeight = 0;
        foreach (LootEntry entry in _lootConfigSO.LootTable)
        {
            totalWeight += entry.Weight;
        }

        return totalWeight;
    }

    LootEntry PickRandomLootFromTable(int totalWeight)
    {
        int cumulative = 0;
        int roll = Random.Range(0, totalWeight);
        foreach (LootEntry entry in _lootConfigSO.LootTable)
        {
            cumulative += entry.Weight;

            if (roll < cumulative)
                return entry;
        }

        return null;
    }

    IEnumerator DropItemInDirections(GameObject item, int itemAmount)
{
    yield return new WaitForEndOfFrame();
    yield return new WaitUntil(() => _rb2D.velocity.magnitude < .15f);


    Vector2[] directions = GetDropDirections(itemAmount);
    for (int i = 0; i < itemAmount; i++)
    {
        GameObject spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
        CogwheelMagnetism script = spawnedItem.GetComponent<CogwheelMagnetism>();
        script.AssignPlayerTransform(_player);
        Rigidbody2D rb = spawnedItem.GetComponent<Rigidbody2D>();
        

        Vector2 dir = directions[i].normalized;
        float speed = Random.Range(4f, 6f);

        rb.velocity = dir * speed;
        rb.drag = 5f;
    }
}

    Vector2[] GetDropDirections(int itemAmount)
    {
        switch (itemAmount)
        {
            case 1:
                return new Vector2[] { Vector2.up };
            case 2:
                return new Vector2[] { Vector2.left, Vector2.right };
            case 3:
                return new Vector2[] { Vector2.left, Vector2.up, Vector2.right };
            case 4:
                return new Vector2[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
            case 5:
                return new Vector2[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down, Vector2.up + Vector2.left };
            default:
                // In case item amount is greater than 5
                Vector2[] dirs = new Vector2[itemAmount];
                for (int i = 0; i < itemAmount; i++)
                {
                    float angle = i * (360f / itemAmount);
                    dirs[i] = Quaternion.Euler(0, 0, angle) * Vector2.right;
                }
                return dirs;
        }
    }

}

