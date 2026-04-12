using UnityEngine;
using UnityEngine.UI;

public class HealthContainer : MonoBehaviour
{
    [SerializeField] Sprite _depletedSprite;
    [SerializeField] Sprite _halfFullSprite;
    [SerializeField] Sprite _fullSprite;
    [SerializeField] Image _imageComponent;
    public HealthbarContainerState ContainerState {get;private set;}

    void Awake()
    {
        UpdateContainer(HealthbarContainerState.DEPLETED);
    }

    

    public void UpdateContainer(HealthbarContainerState newContainerState)
    {
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

    void UpdateContainerSprite(Sprite sprite)
    {
        if (_imageComponent != null)
        {
            if (sprite != null)
                _imageComponent.sprite = sprite;
        }
    }

}


public enum UIHealthContainerType
{
    HEALTH,
    SHIELD
}
public enum HealthbarContainerState
{
    DEPLETED = 0,
    HALF_FULL = 1,
    FULL = 2
}
