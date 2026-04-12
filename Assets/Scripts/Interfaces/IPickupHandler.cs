public interface IPickupHandler { }

public interface IHealthPickupHandler : IPickupHandler
{
    bool CanBeHealed();
    void Heal(int amount);
    void AddShieldHP(int amount);
}

public interface IWeaponPickupHandler : IPickupHandler
{
    //bool CanPickupWeapon(PlayerWeaponConfigSO playerWeaponConfig);
    bool CanPickupWeapon(PlayerWeaponConfigSO playerWeaponConfig);
    //public void AddPickedUpWeapon(WeaponPickupPayload payload);
    public void AddPickedUpWeapon(WeaponPickupPayload payload);
    //bool CanPickupWeapon(PlayerWeapon weapon);
}

public interface IPassiveItemPickupHandler : IPickupHandler
{
    bool CanAddPassiveItem(PassiveItemSO passive);
    void AddPassiveItem(PassiveItemPickupPayload passive);
}

public interface ICurrencyPickupHandler : IPickupHandler
{
    public void AddCurrency(int amount);
}


