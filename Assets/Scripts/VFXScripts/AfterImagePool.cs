using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class AfterImagePool : MonoBehaviour
{
    /*
    ObjectPool<AfterImage> _afterImagePool;
    [SerializeField] GameObject _afterImagePrefab;
    [SerializeField] int initialPoolSize = 10;
    void Awake()
    {
        _afterImagePool = new ObjectPool<AfterImage>(
        createFunc: CreatePooledObject,
        actionOnGet: OnGetFromPool,
        actionOnRelease: OnReleaseToPool,
        actionOnDestroy: OnDestroyPooledObject,
        collectionCheck: false,
        defaultCapacity: 10,
        maxSize: 25
        );
    }

    AfterImage CreatePooledObject()
    {
        GameObject go = Instantiate(_afterImagePrefab);
        AfterImage afterImage = go.GetComponent<AfterImage>();
        afterImage.SetReturnAction(OnReleaseToPool);
        return afterImage;
    }

    void OnGetFromPool(AfterImage afterImage)
    {
        if (afterImage != null)
            afterImage.gameObject.SetActive(true);
    }

    void OnReleaseToPool(AfterImage afterImage)
    {
        if (afterImage != null)
            afterImage.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(AfterImage ai)
    {
        Destroy(ai.gameObject);
    }
    */

}