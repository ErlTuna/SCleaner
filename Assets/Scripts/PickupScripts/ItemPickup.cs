using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ItemPickup : MonoBehaviour, IHighlightable, IInteractable
{
    public Vector3 Location => transform.position;
    public bool IsInteractable {get; private set;} = true;
    protected PickupExecutionContext pickupExecutionContext;

    [Header("Event Channel")]
    [SerializeField] protected ItemPickedUpNotificationEventChannelSO _itemPickedUpNotificationEventChannel;
    [SerializeField] protected AudioCueEventChannelSO _audioCueRequestChannel;

    [Header("Unity Components")]
    [SerializeField] protected BoxCollider2D boxCollider2D;
    [SerializeField] protected SpriteRenderer visualsSR;

    [Header("Materials for Shader")]
    [SerializeField] SpriteOutline2D _outline2D;
    [SerializeField] protected Material normalMaterial;
    [SerializeField] protected Material outlineMaterial;

    [Header("Pickup Config")]
    [SerializeField] protected ItemSO itemConfig;

    [Header("Flags")]

    [Tooltip("If selected, the pickup will adjust its box collider using its sprite's rect. Deselect if you wish to provide a collider size of your own.")]
    [SerializeField] protected bool autoUpdateCollider = true;
    [Tooltip("If selected, the pickup will adjust its sprite using the sprite inside the config. Deselect if you wish to provide a sprite of your own.")]
    [SerializeField] protected bool autoUpdateSprite = false;

    void Awake()
    {
        if (boxCollider2D == null)
            boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        SetupVisuals();
        if (autoUpdateCollider)
            UpdateColliderSize();
    }

    public void OnInteractionAttempt(GameObject interactor)
    {
        if (interactor.TryGetComponent(out PlayerMain player) == false) return;

        IPickupPayload payload = itemConfig.CreatePayload();

        // Check for any IPayloadProvider on this prefab
        if (payload == null)
        {
            if (TryGetComponent<IPayloadProvider>(out var provider))
                payload = provider.CreatePayload();
        }

        PickupTransactionData transactionData = new(itemConfig, payload);
        pickupExecutionContext = new PickupExecutionContext(player, transactionData);

        if (CanBeCollected(pickupExecutionContext))
            OnCollected();
    }

    
    public virtual void OnCollected()
    {
        foreach (PickupBehaviorEntry behaviorEntry in itemConfig.Behaviors)
        {
            if (ItemTracker.Instance.HasBeenConsumed(itemConfig.ItemID) && behaviorEntry.IsExhaustible)
                continue;
            

            behaviorEntry.TryApply(pickupExecutionContext);
        }

        // Notify UI / events
        if (_itemPickedUpNotificationEventChannel != null)
        {

            PickedUpItemData itemData = new()
            {
                name = itemConfig.PickupDefinition.PickUpName,
                description = itemConfig.PickupDefinition.PickUpDescription,
                icon = visualsSR.sprite
            };

            _itemPickedUpNotificationEventChannel.RaiseEvent(itemData);
        }

        if (itemConfig.PickupDefinition.PickUpSuccessSoundData)
            _audioCueRequestChannel.RaiseEvent(itemConfig.PickupDefinition.PickUpSuccessSoundData, transform.position);
        
        ItemTracker.Instance.MarkAsConsumed(itemConfig.ItemID);
        Destroy(gameObject);
    }

    public virtual void SetupVisuals()
    {
        if (visualsSR == null) return;

        if (visualsSR.sprite != null && autoUpdateSprite == false)
        {
            _outline2D.Init();
            return;
        }

        if (itemConfig != null)
        {
            if (itemConfig.PickupDefinition)
            {
                if (itemConfig.PickupDefinition.DefaultPickupSprite != null)
                {
                    visualsSR.sprite = itemConfig.PickupDefinition.DefaultPickupSprite;
                    _outline2D.Init();
                }
            }
        }       
    }

    public virtual bool CanBeCollected(PickupExecutionContext ctx)
    {
        if (itemConfig.GeneralConditions == null) return true;

        foreach (var condition in itemConfig.GeneralConditions)
        {
            if (condition.Evaluate(ctx) == false) 
            {
                Debug.Log("A general condition evaluated to false...");
                return false;
            }
        }
            

        return true;
    }

    protected virtual void UpdateColliderSize()
    {
        if (visualsSR == null) return;
        if (visualsSR.sprite == null) return;
        if (boxCollider2D == null) return;


        Vector2 spriteSize = visualsSR.sprite.bounds.size;
        boxCollider2D.size = spriteSize;
        boxCollider2D.offset = visualsSR.sprite.bounds.center;
    }

    public virtual void Highlight(bool higlighted)
    {
        //if(spriteRenderer != null)
            //spriteRenderer.sharedMaterial = higlighted ? outlineMaterial : normalMaterial;
        
        if (_outline2D)
        {
            _outline2D.Higlight(higlighted);
        }
    }

    public virtual void SetPickupConfig(ItemSO pickupConfig)
    {
        this.itemConfig = pickupConfig;
    }


}

