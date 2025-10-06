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
    [SerializeField] Sprite _fallbackSprite;
    readonly Color TransparentWhite = new(1f, 1f, 1f, 0f);


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
            //_imageComponent.sprite = _fallbackSprite;
            _imageComponent.color = TransparentWhite;
            UpdateAmmoDisplay(null);
            return;
        }


        _imageComponent.sprite = weaponSprite;
        _imageComponent.color = Color.white;
        UpdateAmmoDisplay(runtimeData);
    }

    void UpdateAmmoDisplay(WeaponRuntimeData weaponData)
    {
        if (weaponData == null)
        {
            _ammoText.SetText("");
            return;
        }

        string currentAmmo = weaponData.CurrentAmmo.ToString();
        string currentReserve = weaponData.ReserveAmmo.ToString();
        string text = currentAmmo + "/" + currentReserve;
        _ammoText.SetText(text);
    }





}
