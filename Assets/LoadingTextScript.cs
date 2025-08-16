using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTextScript : MonoBehaviour
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
        yield return new WaitForSecondsRealtime(2.5f);
        SceneManager.LoadScene("Test", LoadSceneMode.Single);
    }
    

}
