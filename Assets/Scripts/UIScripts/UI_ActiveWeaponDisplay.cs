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
        PlayerInventoryManager.OnWeaponSwitchUIUpdate += UpdateWeaponDisplay;
        PlayerInventoryManager.OnWeaponSwitchUIUpdate += UpdateAmmoDisplay;
        WeaponEvents.OnWeaponFiredEvent += UpdateAmmoDisplay;
        WeaponEvents.OnWeaponReloadEvent += UpdateAmmoDisplay;
    }

    void OnDisable()
    {
        PlayerInventoryManager.OnWeaponSwitchUIUpdate -= UpdateWeaponDisplay;
        PlayerInventoryManager.OnWeaponSwitchUIUpdate -= UpdateAmmoDisplay;
        WeaponEvents.OnWeaponFiredEvent -= UpdateAmmoDisplay;
        WeaponEvents.OnWeaponReloadEvent -= UpdateAmmoDisplay;
    }

    void UpdateWeaponDisplay(WeaponData weaponData)
    {
        _imageComponent.sprite = weaponData.sprite;
    }

    void UpdateAmmoDisplay(WeaponData weaponData)
    {
        string currentAmmo = weaponData.currentAmmo.ToString();
        string currentReserve = weaponData.currentReserveAmmo.ToString();
        string text = currentAmmo + "/" + currentReserve;
        _ammoText.SetText(text);
    }




}
