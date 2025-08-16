using UnityEngine;

public class PauseMenu_v2 : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject firstSelectedOption;

    void OnEnable()
    {
        UISelector.instance.SetSelected(firstSelectedOption);
    }

    void Start()
    {
        
    }

}
