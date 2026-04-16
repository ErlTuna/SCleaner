using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActiveWeaponDisplay : MonoBehaviour
{
    [SerializeField] AmmoChangeEventChannelSO _ammoChangeEventChannelSO;
    [SerializeField] WeaponUIUpdateEventChannelSO _weaponChangeEventChannelSO;
    [SerializeField] Image _weaponIcon;
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] Sprite _fallbackSprite;
    readonly Color TransparentWhite = new(1f, 1f, 1f, 0f);


    void OnEnable()
    {
        _weaponChangeEventChannelSO.OnEventRaised += UpdateDisplayOnWeaponSwitch;
        _ammoChangeEventChannelSO.OnEventRaised += UpdateAmmoDisplay;

    }

    void OnDisable()
    {
        _weaponChangeEventChannelSO.OnEventRaised -= UpdateDisplayOnWeaponSwitch;
        _ammoChangeEventChannelSO.OnEventRaised -= UpdateAmmoDisplay;

    }

    void UpdateDisplayOnWeaponSwitch(UIWeaponSwitchContext weaponUpdateData)
    {
        if (weaponUpdateData.weaponIcon != null)
        {
            _weaponIcon.sprite = weaponUpdateData.weaponIcon;
            _weaponIcon.color = Color.white;
        }

        else
        {
            _weaponIcon.color = TransparentWhite;
        }

        AmmoData ammoData = new()
        {
            current = weaponUpdateData.currentAmmo,
            reserve = weaponUpdateData.reserveAmmo,
            hasInfiniteReserve = weaponUpdateData.hasInfiniteReserveAmmo,

        };

        UpdateAmmoDisplay(ammoData);
    }

     void UpdateAmmoDisplay(AmmoData ammoData)
    {
        string currentAmmo = ammoData.current.ToString();
        string currentReserve;
        if (ammoData.hasInfiniteReserve == false)
            currentReserve = ammoData.reserve.ToString();
        
        else
            currentReserve = "INF";

        string text = currentAmmo + "/" + currentReserve;
        _ammoText.SetText(text);
    }


}

/*
    void UpdateWeaponDisplayUI(Sprite weaponIcon, WeaponRuntimeData runtimeData)
    {

        if (weaponIcon == null)
        {
            //_imageComponent.sprite = _fallbackSprite;
            _weaponIcon.color = TransparentWhite;
            //UpdateAmmoDisplay(null);
            return;
        }


        _weaponIcon.sprite = weaponIcon;
        _weaponIcon.color = Color.white;
        //UpdateAmmoDisplay(runtimeData);
    }
*/

/*
void UpdateAmmoDisplay(int current, int reserve)
    {
        string currentAmmo = current.ToString();
        string currentReserve = reserve.ToString();
        string text = currentAmmo + "/" + currentReserve;
        _ammoText.SetText(text);
    }
*/