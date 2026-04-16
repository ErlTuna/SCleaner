using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldContainer : MonoBehaviour
{
    //[SerializeField] Sprite _depletedSprite;
    [SerializeField] Sprite _halfFullSprite;
    [SerializeField] Sprite _fullSprite;
    [SerializeField] Image _imageComponent;
    public ShieldContainerState ContainerState {get;private set;}

    void Awake()
    {
        //UpdateContainer(HealthContainerState.DEPLETED);
    }

    public void UpdateContainer(ShieldContainerState newContainerState)
    {
        switch (newContainerState)
        {
            case ShieldContainerState.DEPLETED :
                ContainerState = newContainerState;
                //UpdateContainerSprite(null);
                break;
            
            case ShieldContainerState.HALF_FULL :
                ContainerState = newContainerState;
                UpdateContainerSprite(_halfFullSprite);
                break;

            case ShieldContainerState.FULL :
                ContainerState = newContainerState;
                UpdateContainerSprite(_fullSprite);
                break;
        }

    }

    void UpdateContainerSprite(Sprite sprite)
    {
        if (_imageComponent != null)
        {
            if (sprite != null)
                _imageComponent.sprite = sprite;
        }
    }

}

public enum ShieldContainerState
{
    DEPLETED = 0,
    HALF_FULL = 1,
    FULL = 2
}
