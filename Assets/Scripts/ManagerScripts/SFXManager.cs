using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] PooledSFXPlayer _player;

    public void Play(SoundDataSO data)
    {
        _player.TryPlaySound(data);
    }

    public void Play(SoundDataSO data, Vector3 location)
    {
        _player.TryPlaySound(data, location);
    }

    public void SetVolume(float volume)
    {
        _player.SetEmitterVolume(volume);
    }
}
