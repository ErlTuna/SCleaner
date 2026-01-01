using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsOverlay : MonoBehaviour, IMenuOverlay
{

    [Header("Panel References")]
    [SerializeField] RectTransform _overlayRectTransform;
    [SerializeField] GameObject _panelTitle;
    [SerializeField] GameObject _panelItems;

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
        _currentTween?.Kill();
        _panelTitle.SetActive(true);
        _currentTween = _overlayRectTransform.
                        DOAnchorPos(_visibleAnchoredTransform.anchoredPosition, _tweenDuration)
                        .SetEase(_tweenEase).OnStart(() =>
                            {                
                                gameObject.SetActive(true);
                            }
                            );
    }

    public void ClosePanel()
    {
        IsVisible = false;
        _currentTween?.Kill();
        _currentTween = _overlayRectTransform.
                        DOAnchorPos(_hiddenAnchoredTransform.anchoredPosition, _tweenDuration)
                        .SetEase(_tweenEase).OnComplete(() =>
                            {                             
                                gameObject.SetActive(false);
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
}
