using UnityEngine;
using UnityEngine.UI;
public class TestingWeaponSwitch : MonoBehaviour
{
    public int currentWeaponSpriteIndex = 0;
    [SerializeField] Sprite[] weaponSprites;
    [SerializeField] Image imageComponent;


    public void CycleForward()
    {
        ++currentWeaponSpriteIndex;
        if (currentWeaponSpriteIndex > weaponSprites.Length - 1)
            currentWeaponSpriteIndex = 0;

        imageComponent.sprite = weaponSprites[currentWeaponSpriteIndex];
    }

    public void CycleBackward()
    {
        --currentWeaponSpriteIndex;
        if (currentWeaponSpriteIndex < 0)
            currentWeaponSpriteIndex = weaponSprites.Length - 1;

        imageComponent.sprite = weaponSprites[currentWeaponSpriteIndex];
    }

}
