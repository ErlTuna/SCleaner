using UnityEngine;

public class TestScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.SetGameState(GameState.LOADING_NEXT_GAMEPLAY_LEVEL);
    }
}
