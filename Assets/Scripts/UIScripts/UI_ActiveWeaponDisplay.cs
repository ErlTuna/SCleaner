using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActiveWeaponDisplay : MonoBehaviour
{
    [SerializeField] Image _imageComponent;
    [SerializeField] TextMeshProUGUI _ammoText;

    void OnEnable()
    {
        PlayerInventoryEvents.OnWeaponSwitchUIUpdate += UpdateWeaponDisplayUI;
        WeaponEvents.OnWeaponFiredEvent += UpdateAmmoDisplay;
        WeaponEvents.OnWeaponReloadEvent += UpdateAmmoDisplay;

    }

    void OnDisable()
    {
        PlayerInventoryEvents.OnWeaponSwitchUIUpdate -= UpdateWeaponDisplayUI;
        WeaponEvents.OnWeaponFiredEvent -= UpdateAmmoDisplay;
        WeaponEvents.OnWeaponReloadEvent -= UpdateAmmoDisplay;

    }

    void UpdateWeaponDisplayUI(Sprite weaponSprite, WeaponRuntimeData runtimeData)
    {

        if (weaponSprite == null)
        {
        Debug.LogWarning("Weapon sprite is null — likely due to weapon not being fully initialized.");
        return;
        }


        _imageComponent.sprite = weaponSprite;
        UpdateAmmoDisplay(runtimeData);
    }

    void UpdateAmmoDisplay(WeaponRuntimeData weaponData)
    {
        string currentAmmo = weaponData.CurrentAmmo.ToString();
        string currentReserve = weaponData.ReserveAmmo.ToString();
        string text = currentAmmo + "/" + currentReserve;
        _ammoText.SetText(text);
    }





}
