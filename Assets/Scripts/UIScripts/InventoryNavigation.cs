using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryNavigation : MonoBehaviour
{
    [SerializeField] private float inputCooldown = 0.2f; // prevent moving too fast
    [SerializeField] float lastInputTime = 0f;
    private void Update()
    {
        if (Time.unscaledTime - lastInputTime < inputCooldown)
            return;

        Vector2 movement = PlayerInputManager.Instance.NavigationInput;

        if (movement.y > 0.5f)
        {
            MoveSelection(Vector2.up);
        }
        else if (movement.y < -0.5f)
        {
            MoveSelection(Vector2.down);
        }
        else if (movement.x < -0.5f)
        {
            MoveSelection(Vector2.left);
        }
        else if (movement.x > 0.5f)
        {
            MoveSelection(Vector2.right);
        }
    }

    private void MoveSelection(Vector2 direction)
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return;

        if (current.TryGetComponent<InventoryEntry>(out var entry) == false) return;

        InventoryEntry next = null;

        if (direction == Vector2.up) next = entry.Up;
        else if (direction == Vector2.down) next = entry.Down;
        else if (direction == Vector2.left) next = entry.Left;
        else if (direction == Vector2.right) next = entry.Right;

        if (next != null)
        {
            EventSystem.current.SetSelectedGameObject(next.gameObject);
            next.OnSelect(null); // optional: trigger selection highlight immediately
            lastInputTime = Time.unscaledTime;
        }
    }
}


