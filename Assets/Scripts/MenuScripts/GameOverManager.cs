using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
 
public class GameOverManager : MenuContext
{

    [Header("Event Channels")]
    [SerializeField] AudioCueEventChannelSO _sfxEventChannel;

    [Header("UI Elements")]
    [SerializeField] Canvas _canvas;
    [SerializeField] CanvasGroup _canvasGroup; 
    [SerializeField] GameObject _titleDisplayGO;
    [SerializeField] TextMeshProUGUI _titleDisplayTMP;
    [SerializeField] TextMeshProUGUI _titleDescTMP;
    [SerializeField] GameObject _killCountGroup;
    [SerializeField] GameObject _timeTakenGroup;
    [SerializeField] TextMeshProUGUI _killCounterText;
    [SerializeField] TextMeshProUGUI _timeTakenCounterText;
    [SerializeField] Button _proceedToNextLevelButton;
    [SerializeField] Button _returnToMainMenuButton;
    [SerializeField] Button _quitButton;
 
    [Header("Counter Values")]
    [SerializeField] int _killCounterTargetValue = 1000;
    [SerializeField] float _timeCounterTargetValue = 500;
 
    [Header("Speed Settings")]
    [SerializeField] float _normalSpeed = 10;
    [SerializeField] float _fastSpeed = 30f;

    [Header("Delays")]
    [SerializeField] float _titleAppearanceDelay = 0.5f;
    [SerializeField] float _killCounterAppearanceDelay = 0.5f;
    [SerializeField] float _timeTakenCounterAppearanceDelay = 0.5f;

    [Header("Flags")] 
    [SerializeField] bool _sequencePlaying = false;
    bool _isProcessingFormattedCounter = false;
    [SerializeField] bool _skipPressed = false;
    bool _isLevelOverContext = false;
    bool _isGameOverContext = false;
    bool _isGameEndContext = false;

    [Header("Misc")]
    [SerializeField] SoundDataSO _popSFX;

    void Awake()
    {
        //GameManager.OnGameOverShowGameOverMenu += ShowGameOverMenu;
        //GameManager.OnLevelOverShowLevelEndMenu += ShowLevelCompletedMenu;
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    void OnEnable()
    {
        GameManager.OnGameOverShowGameOverMenu += ShowGameOverMenu;
        GameManager.OnLevelOver += ShowLevelCompletedMenu;
        GameManager.OnGameCompleted += ShowGameCompletedMenu;
    }

    void OnDisable()
    {
        
        GameManager.OnGameOverShowGameOverMenu -= ShowGameOverMenu;
        GameManager.OnLevelOver -= ShowLevelCompletedMenu;
        GameManager.OnGameCompleted -= ShowGameCompletedMenu;
    }
 
    void Update()
    {
        if (_sequencePlaying)
        {
            _skipPressed = PlayerInputManager.Instance.SubmitPressed;
        }
    }

    void ShowGameOverMenu()
    {   
        if (_canvasGroup != null)
        {
            
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            _proceedToNextLevelButton.interactable = false;
            _quitButton.interactable = false;
            _returnToMainMenuButton.interactable = false;

            PrepareUIForPlayerDefeat();
            SetCurrentContext(this);
            StartCoroutine(MenuDisplaySequence());
        }
    }

    void ShowLevelCompletedMenu()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            _returnToMainMenuButton.interactable = false;
            _proceedToNextLevelButton.interactable = false;
            _quitButton.interactable = false;

            PrepareUIForLevelCompleted();
            SetCurrentContext(this);
            StartCoroutine(MenuDisplaySequence());
        }
    }

    void ShowGameCompletedMenu()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            _returnToMainMenuButton.interactable = false;

            PrepareUIForGameCompleted();
            SetCurrentContext(this);
            StartCoroutine(MenuDisplaySequence());
        }
    }
 
    IEnumerator MenuDisplaySequence()
    {
        _sequencePlaying = true;

        // Enable title after some delay
        yield return SetActiveAfterDelay(_titleDisplayGO, _titleAppearanceDelay);
        if (_sfxEventChannel != null)
            _sfxEventChannel.RaiseEvent(_popSFX, transform.position);

        // Kill Counter (Counter #1)
        // -----------------------------
        // Enable kill counter after some delay then start counting
        _killCounterTargetValue = StatTracker.Instance.KillCount;
        yield return SetActiveAfterDelay(_killCountGroup, _killCounterAppearanceDelay);
        yield return StartCoroutine(CountUp(_killCounterText, _killCounterTargetValue));
        // -----------------------------
 
        // Time Taken (Counter #2)
        // -----------------------------
        // Enable time counter after some delay then start counting
        yield return SetActiveAfterDelay(_timeTakenGroup, _timeTakenCounterAppearanceDelay);
        _isProcessingFormattedCounter = true;
        _timeCounterTargetValue = StatTracker.Instance.ElapsedTime;
        yield return StartCoroutine(CountUp(_timeTakenCounterText, _timeCounterTargetValue));
        // -----------------------------


        // Enable Return to Main Menu/Proceed to Next Level button when both done
        if (_isGameOverContext)
        {
            _returnToMainMenuButton.interactable = true;
            _quitButton.interactable = true;
            UISelector.instance.SetSelected(_returnToMainMenuButton.gameObject);
        }
        else if (_isGameOverContext == false && _isLevelOverContext)
        {
            _proceedToNextLevelButton.interactable = true;
            _returnToMainMenuButton.interactable = true;
            UISelector.instance.SetSelected(_proceedToNextLevelButton.gameObject);
        }
        else
        {
            _returnToMainMenuButton.interactable = true;
            _quitButton.interactable = true;
            UISelector.instance.SetSelected(_returnToMainMenuButton.gameObject);
        }
        
        _sequencePlaying = false;
        
    }

    
    IEnumerator CountUp(TextMeshProUGUI textField, float targetValue)
    {
        float current = 0f;
        int valueToDisplay = 0;

        while (valueToDisplay < targetValue)
        {
            // Handle skip
            if (_skipPressed)
            {
                valueToDisplay = Mathf.RoundToInt(targetValue);
                break;
            }

            // Increment current value
            current += _normalSpeed * Time.deltaTime;
            valueToDisplay = Mathf.RoundToInt(Mathf.Min(current, targetValue));

            // Update the display
            if (_isProcessingFormattedCounter)
                UpdateFormattedTime(valueToDisplay);
            else
                textField.text = valueToDisplay.ToString();

            yield return null;
        }

        // Ensure final value is exactly targetValue
        if (!_isProcessingFormattedCounter)
            textField.text = valueToDisplay.ToString();
        else
            UpdateFormattedTime(valueToDisplay);
    }

    // ----------------------------
    // HELPERS
    // ----------------------------
    IEnumerator SetActiveAfterDelay(GameObject targetGO, float delay)
    {
        yield return new WaitForSeconds(delay);
        targetGO.SetActive(true);
    }

    void UpdateFormattedTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        _timeTakenCounterText.text = $"{minutes:00}:{seconds:00}";
    }

    public override void Execute(MenuAction action)
    {
        switch (action)
        {   
            case MenuAction.ProceedToNextLevel:
            GameManager.Instance.SetGameState(GameState.LOADING_NEXT_GAMEPLAY_LEVEL);
            break;

            case MenuAction.ReturnToMainMenu:
            GameManager.Instance.SetGameState(GameState.RETURNING_TO_MAIN_MENU);
            break;

            case MenuAction.QuitGame:
            GameManager.Instance.SetGameState(GameState.SHUTTING_DOWN);
            break;
        }
        
    }

    void PrepareUIForPlayerDefeat()
    {
        LinkButtons(_returnToMainMenuButton, _quitButton);
        _proceedToNextLevelButton.gameObject.SetActive(false);
        _returnToMainMenuButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
        _titleDisplayTMP.text = "YOU FAILED";
        _titleDescTMP.SetText("The house remains dirty still");
        _isGameOverContext = true;
        _isLevelOverContext = false;
    }

    void PrepareUIForLevelCompleted()
    {
        //LinkButtons(_proceedToNextLevelButton, _quitButton);
        LinkButtons(_proceedToNextLevelButton, _returnToMainMenuButton);
        _quitButton.gameObject.SetActive(false);
        _returnToMainMenuButton.gameObject.SetActive(true);
        _proceedToNextLevelButton.gameObject.SetActive(true);
        _titleDisplayTMP.text = "YOU SUCCEEDED";
        _titleDescTMP.SetText("Your work is done here");
        _isGameOverContext = false;
        _isLevelOverContext = true;
    }

    void PrepareUIForGameCompleted()
    {
        _proceedToNextLevelButton.gameObject.SetActive(false);
        _returnToMainMenuButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
        LinkButtons(_returnToMainMenuButton, _quitButton);
        _titleDisplayTMP.text = "YOU WON";
        _titleDescTMP.SetText("Everything is clean now");
        _isGameEndContext = true;
    }

    void LinkButtons(Button left, Button right)
    {
        Navigation leftNav = left.navigation;
        leftNav.mode = Navigation.Mode.Explicit;
        leftNav.selectOnRight = right;
        left.navigation = leftNav;

        Navigation rightNav = right.navigation;
        rightNav.mode = Navigation.Mode.Explicit;
        rightNav.selectOnLeft = left;
        right.navigation = rightNav;
    }

    // ----------------------------
}
