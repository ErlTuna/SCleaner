using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] AudioSource _BGMSource;
    [SerializeField] AudioClip _BGMClip;
    [SerializeField] SceneMusicLibrarySO _sceneMusicLibrary;

    void OnEnable()
    {
        GameManager.OnLevelLoadedMusicRequest += SetBGMByScene;
    }

    void OnDisable()
    {
        GameManager.OnLevelLoadedMusicRequest -= SetBGMByScene;
    }

    public void SetBGMByScene(SceneReference sceneRef)
    {
        if (_BGMSource.clip == _sceneMusicLibrary.GetMusic(sceneRef))
            return;

            
        _BGMSource.clip = _sceneMusicLibrary.GetMusic(sceneRef);
        _BGMSource.loop = true;
        _BGMSource.Play();
    }

    public void PlayBGM()
    {
        _BGMSource.clip = _sceneMusicLibrary.GetMusic(GameManager.Instance.CurrentGameplaySceneRef);
        _BGMSource.loop = true;
        _BGMSource.Play();
    }

    public void SetVolume(float volume)
    {
        _BGMSource.volume = volume;
    }
}
