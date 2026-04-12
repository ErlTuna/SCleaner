using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemNotificationBar : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] ItemPickedUpNotificationEventChannelSO _itemPickedUpEventChannelSO;
    [SerializeField] TMP_Text _itemName;
    [SerializeField] Image _itemIcon;
    [SerializeField] TMP_Text _itemDescription;
    [SerializeField] CanvasGroup _canvasGroup;

    [Header("Animation")]
    [SerializeField] float _slideDistance = 80f;
    [SerializeField] float _slideDuration = 0.35f;
    [SerializeField] float _visibleTime = 2f;

    Vector2 _startPos;
    Tween _moveTween;

    void Start()
    {
        _startPos = _rectTransform.anchoredPosition;

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    void OnEnable()
    {
        _itemPickedUpEventChannelSO.OnEventRaised += OnItemPickedUp;
    }

    void OnDisable()
    {
        _itemPickedUpEventChannelSO.OnEventRaised -= OnItemPickedUp;
    }


    public void OnItemPickedUp(PickedUpItemData itemData)
    {

        gameObject.SetActive(true);

        Debug.Log("An item is picked up.");


        if (_itemName != null) _itemName.text = itemData.name;

        if (_itemIcon != null)
        {
            _itemIcon.sprite = itemData.icon;
            //_itemIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemData.icon.rect.width);
            //_itemIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemData.icon.rect.height);
        }

        if (_itemDescription != null) _itemDescription.text = itemData.description;
        Show();    
    }

    void Show()
    {
        _moveTween?.Kill();

        // Reset state.
        _rectTransform.anchoredPosition = _startPos;
        _canvasGroup.alpha = 1f;

        // Slide down after visible time passes.
        _moveTween = DOVirtual.DelayedCall(_visibleTime, SlideDown);
    }

    void SlideDown()
    {
        _moveTween?.Kill();

        _moveTween = _rectTransform
            .DOAnchorPosY(_startPos.y - _slideDistance, _slideDuration)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                _canvasGroup.alpha = 0f;
            });
    }

}


