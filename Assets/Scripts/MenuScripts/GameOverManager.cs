using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class GameOverManager : MenuContext
{

    [Header("Event Channels")]
    [SerializeField] AudioCueEventChannelSO _sfxEventChannel;

    [Header("UI Elements")]
    [SerializeField] Canvas _canvas;
    [SerializeField] CanvasGroup _canvasGroup; 
    [SerializeField] GameObject _titleDisplay;
    [SerializeField] GameObject _killCountGroup;
    [SerializeField] GameObject _timeTakenGroup;
    [SerializeField] TextMeshProUGUI _killCounterText;
    [SerializeField] TextMeshProUGUI _timeTakenCounterText;
    [SerializeField] Button _returnToMainMenuButton;
 
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

    [Header("Misc")]
    [SerializeField] SoundDataSO _popSFX;
    void Awake()
    {
        GameManager.OnGameOverShowGameOverMenu += ShowGameOverMenu;
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    void OnDisable()
    {
        GameManager.OnGameOverShowGameOverMenu -= ShowGameOverMenu;
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
            
            _returnToMainMenuButton.interactable = false;
            SetCurrentContext(this);
            StartCoroutine(GameOverSequence());
        }
    }
 
    IEnumerator GameOverSequence()
    {
        _sequencePlaying = true;

        // Enable title after some delay
        yield return SetActiveAfterDelay(_titleDisplay, _titleAppearanceDelay);
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


        // Enable Return to Main Menu button when both done
        _returnToMainMenuButton.interactable = true;
        UISelector.instance.SetSelected(_returnToMainMenuButton.gameObject);
        
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
            case MenuAction.ReturnToMainMenu:
            GameManager.Instance.SetGameState(GameState.RETURNING_TO_MAIN_MENU);
            break;

            case MenuAction.QuitGame:
            GameManager.Instance.SetGameState(GameState.SHUTTING_DOWN);
            break;
        }
        
    }

    // ----------------------------
}
