using UnityEngine;
using DG.Tweening;

public class LootChestController : MonoBehaviour, IHighlightable, IInteractable
{

    public Vector3 Location => transform.position;
    public bool IsInteractable {get; private set;} = true;
    
    //[SerializeField] SpriteRenderer _visualsSR;
    [SerializeField] SpriteOutline2D _outline2D;

    [Header("Materials for Shader")]
    [SerializeField] protected Material normalMaterial;
    [SerializeField] protected Material outlineMaterial;

    [SerializeField] Animator _animator;
    [SerializeField] GameObject _lootItem;

    [Header("Animation targets")]
    [SerializeField] Transform _targetAir;
    [SerializeField] Transform _targetGround;

    [SerializeField] bool _hasBeenOpened = false;


    void Start()
    {
        _outline2D.Init();
    }

    public void AssignLootItem(GameObject lootItem)
    {
        _lootItem = lootItem;
    }


    public void Highlight(bool higlighted)
    {
        //if(_visualsSR != null)
            //_visualsSR.sharedMaterial = higlighted ? outlineMaterial : normalMaterial;

        if (_outline2D)
        {
            _outline2D.Higlight(higlighted);
        }
    }

    public void OnCollected()
    {
        _hasBeenOpened = true;
        IsInteractable = false;
        _animator.SetTrigger("Open");


        
        Transform lootTransform = _lootItem.transform;
        lootTransform.position = transform.position;
        _lootItem.SetActive(true);

        Sequence seq = DOTween.Sequence();

        Tween upwardMotion = lootTransform.DOMoveY(_targetAir.position.y, .5f, false);
        seq.Append(upwardMotion);

        Tween downwardMotion = lootTransform.DOMoveY(_targetGround.position.y, 1f, false).SetDelay(0.25f);
        seq.Append(downwardMotion);

        seq.SetLink(_lootItem);

        seq.Play();

    }

    public void OnInteractionAttempt(GameObject collector)
    {
        if (_hasBeenOpened == true) return;
            OnCollected();
    }
}
