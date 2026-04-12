using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    ReloadContext _currentReloadContext;
    [SerializeField] ReloadStrategySO _reloadStrategy;
    [SerializeField] AmmoChangeEventChannelSO _ammoChangeEventChannelSO;
    WeaponRuntimeAmmoData _ammoData;

    public void Initialize(WeaponRuntimeAmmoData ammoData)
    {
        _ammoData = ammoData;
        Debug.Log("AmmoManager initialized with AMMODATA...");
    }

    public void SetReloadContext(ReloadContext reloadContext)
    {
        _currentReloadContext = reloadContext;
    }

    public void UseAmmo(int amount = 1)
    {
        if (_ammoData.CurrentAmmo <= 0) return;

        _ammoData.CurrentAmmo -= amount;
        Debug.Log("Consumed ammo. Now ammo is : " + _ammoData.CurrentAmmo);

        RaiseAmmoChangeEvent();
    }

    // This gets called at the end of an animation (invoked by concrete weapon)
    public void UseReloadStrategy()
    {
        _reloadStrategy.PerformReload(_currentReloadContext);
        RaiseAmmoChangeEvent();
    }

    void RaiseAmmoChangeEvent()
    {
        if (_ammoChangeEventChannelSO == null) return;

        AmmoData newAmmoData = new()
        {
            current = _ammoData.CurrentAmmo,
            reserve = _ammoData.CurrentReserveAmmo,
            hasInfiniteReserve = _ammoData.HasInfiniteReserveAmmo
            

        };

        _ammoChangeEventChannelSO.RaiseEvent(newAmmoData);
    }

    # region Helper Methods

    public bool CanReload()
    {
        Debug.Log("Ammo before reload check : " + _ammoData.CurrentAmmo);
        return HasReserveAmmo() && !IsFullyLoaded();
    }
    public bool IsFullyLoaded()
    {
        return _ammoData.CurrentAmmo >= _ammoData.RoundCapacity;
    }

    public bool HasAmmo()
    {
        if (_ammoData == null)
        {
            Debug.Log("ammmo data is null.");
            return false;
        }
        return _ammoData.CurrentAmmo > 0;
    }

    public bool HasReserveAmmo()
    {
        return _ammoData.CurrentReserveAmmo > 0 || _ammoData.HasInfiniteReserveAmmo;
    }

    public void AddReserveAmmo(AmmoPickupType ammoPickupType)
    {
        int temp;
        switch (ammoPickupType)
        {
            case AmmoPickupType.SMALL:
            temp = _ammoData.CurrentReserveAmmo + Mathf.RoundToInt(_ammoData.MaxReserveAmmo * .25f);
            _ammoData.CurrentReserveAmmo = Mathf.Min(_ammoData.MaxReserveAmmo, temp);
            break;

            case AmmoPickupType.MEDIUM:
            temp = _ammoData.CurrentReserveAmmo + Mathf.RoundToInt(_ammoData.MaxReserveAmmo * .5f);
            _ammoData.CurrentReserveAmmo = Mathf.Min(_ammoData.MaxReserveAmmo, temp);
            break;

            case AmmoPickupType.LARGE:
            _ammoData.CurrentReserveAmmo = _ammoData.MaxReserveAmmo;
            break;
        }

        RaiseAmmoChangeEvent();
    }

    public bool CanPickupReserveAmmo()
    {
        return _ammoData.HasInfiniteReserveAmmo == false && _ammoData.CurrentReserveAmmo != _ammoData.MaxReserveAmmo;
    }


    bool HasValidReloadContext()
    {
        return _currentReloadContext != null;
    }

    bool HasValidStrategy()
    {
        return _currentReloadContext != null;
    }

    # endregion
}
