using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] Animator _animator;

    void Start()
    {
        if (_animator != null)
            _animator.Play("Loading");
    }
    

    
}


