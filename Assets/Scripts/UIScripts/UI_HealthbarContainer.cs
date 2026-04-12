using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthbarContainer : MonoBehaviour
{
    [SerializeField] Sprite _depletedSprite;
    [SerializeField] Sprite _halfFullSprite;
    [SerializeField] Sprite _fullSprite;
    [SerializeField] Image _imageComponent;
    [SerializeField] UIHealthContainerType containerType;
    [SerializeField] bool canBeDepleted = true;
    public HealthbarContainerState ContainerState {get;private set;}

    void Awake()
    {
        UpdateContainer(HealthbarContainerState.DEPLETED);
    }

    

    public void UpdateContainer(HealthbarContainerState newContainerState)
    {
        Debug.Log("Update Container called.");
        switch (newContainerState)
        {
            case HealthbarContainerState.DEPLETED :
                ContainerState = newContainerState;
                UpdateContainerSprite(_depletedSprite);
                break;
            
            case HealthbarContainerState.HALF_FULL :
                ContainerState = newContainerState;
                UpdateContainerSprite(_halfFullSprite);
                break;

            case HealthbarContainerState.FULL :
                ContainerState = newContainerState;
                UpdateContainerSprite(_fullSprite);
                break;
        }

    }

    public void AddToContainer()
    {
        switch (ContainerState)
        {
            case HealthbarContainerState.DEPLETED :
            ContainerState = HealthbarContainerState.HALF_FULL;
            UpdateContainerSprite(_halfFullSprite);
            break;

            case HealthbarContainerState.HALF_FULL :
            ContainerState = HealthbarContainerState.FULL;
            UpdateContainerSprite(_fullSprite);
            break;

            default:
            Debug.Log("Invalid action.");
            break;
        }
    }

    public void SubtractFromContainer()
    {
        switch (ContainerState)
        {
            case HealthbarContainerState.FULL :
            ContainerState = HealthbarContainerState.HALF_FULL;
            UpdateContainerSprite(_halfFullSprite);
            break;

            case HealthbarContainerState.HALF_FULL :
            ContainerState = HealthbarContainerState.DEPLETED;
            UpdateContainerSprite(_depletedSprite);
            break;
        }
    }

    void UpdateContainerSprite(Sprite sprite)
    {
        if (_imageComponent)
        {
            if (sprite)
            {
                //Debug.Log("Updated SPRITE!");
                _imageComponent.sprite = sprite;
            }
                
        }
    }
}
