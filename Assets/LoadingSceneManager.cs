using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        if (_animator != null)
            _animator.Play("Loading");
        StartCoroutine(LoadLevel());
    }



    // Update is called once per frame
    IEnumerator LoadLevel()
    {

        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadSceneAsync("GameplayUI", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Test", LoadSceneMode.Additive);
    }
    
    

}
