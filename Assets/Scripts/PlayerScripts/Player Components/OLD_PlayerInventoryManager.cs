using UnityEngine;


public class OLD_PlayerInventoryManager
{
    /*
    Unit _owner;
    PlayerInventoryData _inventoryData;
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    //public List<GameObject> WeaponGOs;
    //public List<BaseWeapon> weaponScripts;
    public GameObject currentWeapon;
    public BaseWeapon_v2 currentWeaponScript;
    public int weaponCount;
    public int currentlyUsedSlot = 0;

    void Awake()
    {
        //PrepareWeaponGOs();
        //weaponCount = _inventoryData.WeaponsHeld;
    }

    void Start()
    {
        //PrepareWeaponScripts();


        //OnInventoryReadyEvent?.Invoke(currentWeapon, currentWeaponScript);
    }

    void Update()
    {
        if (PlayerInputManager.instance.FirstWeaponInput)
            SwitchWeapons(0);
        else if (PlayerInputManager.instance.SecondWeaponInput)
            SwitchWeapons(1);
        else if (PlayerInputManager.instance.ThirdWeaponInput)
            SwitchWeapons(2);
        else if (PlayerInputManager.instance.FourthWeaponInput)
            SwitchWeapons(3);
    }

    public void InitializeWithData(Unit owner, PlayerInventoryData inventoryData)
    {
        _owner = owner;
        _inventoryData = inventoryData;

        foreach (var weaponConfig in _inventoryData.WeaponConfigs)
        {
            GameObject weaponGO = Instantiate(weaponConfig.prefab);
            weaponGO.transform.parent = _weaponParent;
            weaponGO.transform.position = _weaponPosition.position;

            BaseWeapon_v2 weaponScript = weaponGO.GetComponent<BaseWeapon_v2>();
            weaponGO.transform.localPosition += weaponScript.weaponConfig.offset;

            if (weaponScript != null)
            {
                _inventoryData.WeaponGOs.Add(weaponGO);
                _inventoryData.WeaponScripts.Add(weaponScript);
                weaponGO.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Weapon prefab '{weaponConfig.weaponName}' has no BaseWeapon script!");
            }
        }
        SetDefaultWeapon();
        //PlayerInventoryEvents.OnInventoryReadyEvent(currentWeapon, currentWeaponScript);

    }

    void SwitchWeapons(int slot)
    {
        if (currentWeaponScript.weaponConfig.state == WeaponState.FIRING) return;

        int slotToSwitchTo = -1;

        switch (slot)
        {
            case 0:
                slotToSwitchTo = 0;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }

                break;
            case 1:
                slotToSwitchTo = 1;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }

                break;
            case 2:
                slotToSwitchTo = 2;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }
                break;
            case 3:
                slotToSwitchTo = 3;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }
                break;
            default:
                break;
        }
    }

    void WeaponSlotChange(int currentSlot, int slotToSwitchTo)
    {
        
        GameObject targetWeapon = _inventoryData.WeaponGOs[slotToSwitchTo];
        BaseWeapon targetWeaponScript = _inventoryData.WeaponScripts[slotToSwitchTo];
        //Sprite targetWeaponSprite = weaponScripts[slotToSwitchTo].weaponConfig.sprite;
        //WeaponRuntimeData targetWeaponRuntimeData = weaponScripts[slotToSwitchTo].weaponRuntimeData;

        PlayerInventoryEvents.RaiseWeaponSwitchEvent(targetWeapon, targetWeaponScript);
        //OnWeaponSwitchEvent?.Invoke(targetWeapon, targetWeaponScript);
        //OnWeaponSwitchUIUpdate?.Invoke(targetWeaponSprite, targetWeaponRuntimeData);

        currentWeapon = _inventoryData.WeaponGOs[slotToSwitchTo];
        currentWeaponScript = _inventoryData.WeaponScripts[slotToSwitchTo];
        currentlyUsedSlot = slotToSwitchTo;
        
    }

    void PrepareWeaponGOs()
    {
        GameObject instantiatedWeaponPrefab;
        for (int i = 0; i < _inventoryData.WeaponsHeld; ++i)
        {
            if (_inventoryData.WeaponConfigs.ElementAt(i))
            {
                instantiatedWeaponPrefab = Instantiate(_inventoryData.WeaponConfigs[i].prefab);

                WeaponGOs.Add(instantiatedWeaponPrefab);
                instantiatedWeaponPrefab.SetActive(false);
            }
        }
    }

    void PrepareWeaponScripts()
    {
        BaseWeapon instantiatedWeaponScript;
        for (int i = 0; i < weaponCount; ++i)
        {
            if (_weaponParent != null)
            {

                instantiatedWeaponScript = WeaponGOs.ElementAt(i).GetComponent<BaseWeapon>();
                if (instantiatedWeaponScript)
                    weaponScripts.Add(instantiatedWeaponScript);

                WeaponGOs[i].transform.parent = _weaponParent;
                WeaponGOs[i].transform.position = _weaponPosition.position;

                WeaponGOs[i].transform.localPosition += instantiatedWeaponScript.weaponConfig.offset;
                //WeaponGOs[i].SetActive(false);
            }
        
        }
    }

    void SetDefaultWeapon()
    {
        Debug.Log("called");
        currentWeapon = _inventoryData.WeaponGOs.ElementAt(currentlyUsedSlot);
        currentWeaponScript = _inventoryData.WeaponScripts.ElementAt(currentlyUsedSlot);
        StartCoroutine(Test());
    }

    IEnumerator Test()
{
    yield return new WaitForSeconds(1f);
    //PlayerInventoryEvents.RaiseInventoryReadyEvent(currentWeapon, currentWeaponScript);
}

    */
}
    


