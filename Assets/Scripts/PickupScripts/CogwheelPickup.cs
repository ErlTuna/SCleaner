using UnityEngine;

public class CogwheelPickup : MonoBehaviour
{
    [SerializeField] int value;
    void OnTriggerEnter2D(Collider2D collision)
    {
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
