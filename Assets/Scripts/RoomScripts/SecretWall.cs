using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField] GameObject _secretRoom;
    [SerializeField] GameObject _secretWall;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] BoxCollider2D _boxCollider2D;

    [SerializeField] bool _revealed = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   
            if (_revealed == false)
            {
                _revealed = true;
                _spriteRenderer.enabled = false;
                _secretRoom.gameObject.SetActive(true);
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_revealed)
            {
                _revealed = false;
                _spriteRenderer.enabled = true;
                _secretRoom.gameObject.SetActive(false);
            }
            
        }

    }
}
