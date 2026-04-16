using UnityEngine;

public class CogwheelPickup : MonoBehaviour
{
    [SerializeField] int value;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with something!!!" + collision.gameObject.name);
        if (collision.gameObject.TryGetComponent(out PlayerMain playerMain))
        {
            if(playerMain.TryGetManager(out ICurrencyPickupHandler currencyPickupHandler))
            {
                currencyPickupHandler.AddCurrency(value);
                Destroy(gameObject);
            }
        }
    }
}
