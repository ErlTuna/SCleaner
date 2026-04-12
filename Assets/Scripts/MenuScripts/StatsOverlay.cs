using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsOverlay : MonoBehaviour, IMenuOverlay
{

    [Header("Panel References")]
    [SerializeField] RectTransform _overlayRectTransform;
    [SerializeField] GameObject _panelTitle;
    [SerializeField] GameObject _panelItems;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] TextMeshProUGUI _killCounterText;
    [SerializeField] TextMeshProUGUI _timeTakenCounterText;

    [Header("Animation Settings")]
    [SerializeField] float _tweenDuration = 0.5f;
    [SerializeField] Ease _tweenEase = Ease.OutCubic;

    [Header("Panel Positions")]
    [SerializeField] RectTransform _visibleAnchoredTransform;
    [SerializeField] RectTransform _hiddenAnchoredTransform;
    Tween _currentTween;
    public bool IsVisible { get; set;} = false;

    public void OpenPanel()
    {
        IsVisible = true;
        RefreshDisplays();
        _currentTween?.Kill();
        _panelTitle.SetActive(true);
        _currentTween = _overlayRectTransform.
                        DOAnchorPos(_visibleAnchoredTransform.anchoredPosition, _tweenDuration)
                        .SetEase(_tweenEase).
                        SetUpdate(UpdateType.Normal, true).
                        OnStart(() =>
                            {   
                                _canvasGroup.alpha = 1f;
                                _canvasGroup.interactable = true;
                                _canvasGroup.blocksRaycasts = true;
                            }
                            );
    }

    public void ClosePanel()
    {
        IsVisible = false;
        _currentTween?.Kill();
        _currentTween = _overlayRectTransform.
                        DOAnchorPos(_hiddenAnchoredTransform.anchoredPosition, _tweenDuration)
                        .SetEase(_tweenEase).
                        SetUpdate(UpdateType.Normal, true).
                        OnComplete(() =>
                            {                             
                                _canvasGroup.alpha = 0f;
                                _canvasGroup.interactable = false;
                                _canvasGroup.blocksRaycasts = false;
                            }
                            );
    }

    public void Show()
    {
        if (IsVisible) return;
        OpenPanel();
    }
    public void Hide()
    {
        if (!IsVisible) return;
        ClosePanel();
    }

    void UpdateKillCounterDisplay()
    {
        _killCounterText.text = StatTracker.Instance.KillCount.ToString();
        _timeTakenCounterText.text = StatTracker.Instance.ElapsedTime.ToString();
    }

    void UpdateElapsedTimeDisplay()
    {
        float totalSeconds = StatTracker.Instance.ElapsedTime;
        int minutes = Mathf.RoundToInt(totalSeconds / 60);
        int seconds = Mathf.RoundToInt(totalSeconds % 60);
        _timeTakenCounterText.text = $"{minutes:00}:{seconds:00}";
    }

    public void RefreshDisplays()
    {
        UpdateKillCounterDisplay();
        UpdateElapsedTimeDisplay();
    }
}
