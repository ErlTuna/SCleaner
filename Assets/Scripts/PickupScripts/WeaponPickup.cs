using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IPickup
{
    public Vector3 Location => transform.position;
    //[SerializeField] Transform _root;
    WeaponRuntimeData _weaponRuntimeData;
    [SerializeField] WeaponConfigSO _weaponConfig;
    [SerializeField] BoxCollider2D _boxCollider2D;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Material normalMaterial;
    [SerializeField] Material outlineMaterial;

    // This bool marks whether or not a pickup is hand-placed/dynamically created
    // Dropped weapons are converted into weapon pick ups, as such
    // this bool is used to distinguish between the two 
    [SerializeField] bool _isWorldPickup;

    void Awake()
    {
        _spriteRenderer.material = normalMaterial;
    }

    void Start()
    {
        if (_isWorldPickup)
            Initialize(_weaponConfig);


        //_spriteRenderer.flipY = transform.rotation.z > 90f && transform.rotation.z < 270f;

        UpdateColliderSize();
    }


    /*
    public void ShowPrompt()
    {
        if (promptInstance != null)
            promptInstance.Show();

        Debug.Log("Displaying prompt");
    }
    */

    /*
    public void HidePrompt()
    {
        if (promptInstance != null)
            promptInstance.Hide();
    }
    */


    public void OnPickupAttempt(GameObject collector)
    {

        if (CanBePickedUp(collector))
            OnCollected(collector);

        else
            Debug.Log("Can't pick up...");

    }

    public bool CanBePickedUp(GameObject collector)
    {
        if (collector.TryGetComponent(out PlayerInventoryManager playerInventory))
        {
            if (playerInventory.CanPickupWeapon(_weaponConfig))
                return true;
        }

        return false;
    }

    public void OnCollected(GameObject collector)
    {
        PlayerInventoryManager inventory = collector.GetComponentInParent<PlayerInventoryManager>();
        if (inventory == null)
        {
            Debug.Log("Inventory null?..");
            return;
        }


        if (_isWorldPickup)
            inventory.TryAddWeapon(_weaponConfig);

        else
            inventory.TryAddWeapon(_weaponRuntimeData);

        HighlightPickup(false);

        Destroy(gameObject);

    }

    public void Initialize(WeaponConfigSO config)
    {
        if (config == null)
        {
            Debug.LogError("WeaponConfig is null in WeaponPickup initialization. Can't create pickup.");
            Destroy(gameObject);
            return;
        }

        _weaponConfig = config;
        _spriteRenderer.sprite = _weaponConfig.Sprite;

        SetupVisualsAndPrompt();
    }

    public void Initialize(WeaponRuntimeData runtimeData)
    {
        if (runtimeData == null || runtimeData.Config == null)
        {
            Debug.LogError("WeaponConfig is null in WeaponPickup initialization. Can't create pickup.");
            Destroy(gameObject);
            return;
        }

        _weaponRuntimeData = runtimeData;
        _weaponConfig = runtimeData.Config;
        _isWorldPickup = false;
        SetupVisualsAndPrompt();
    }

    private void SetupVisualsAndPrompt()
    {

        _spriteRenderer.sprite = _weaponConfig.Sprite;
        /*
        if (promptInstance == null && promptPrefab != null)
        {
            GameObject promptGO = Instantiate(promptPrefab, _root);
            promptGO.transform.localPosition = _promptPosition.localPosition;

            promptInstance = promptGO.GetComponent<UI_PickupPrompt>();
            promptInstance.Hide();
        }
        */
    }

    public void UpdateColliderSize()
    {
        if (_spriteRenderer.sprite == null) return;


        Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;
        _boxCollider2D.size = spriteSize;
        _boxCollider2D.offset = _spriteRenderer.sprite.bounds.center;
    }

    public void HighlightPickup(bool higlighted)
    {
        if(_spriteRenderer != null)
            _spriteRenderer.material = higlighted ? outlineMaterial : normalMaterial;
    }


}


