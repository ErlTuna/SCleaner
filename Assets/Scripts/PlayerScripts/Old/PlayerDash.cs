using System.Collections;
using UnityEngine;


// Deprecated
public class PlayerDash : MonoBehaviour
{
    /*
    public delegate IEnumerator OnDashTriggered(UnitInfo playerInfo, float dashDuration);
    public static OnDashTriggered onDashTriggered;
    [SerializeField] Rigidbody2D _rb2D;
    [SerializeField] AbilityData _abilityData;
    UnitInfo _playerInfo;
    AudioClip _dashSFX;
    float _dashTime = 0f;  
    int _dashCount = 3; 
    float _timeSinceLastDash;
    int _remainingDashes;
    

    void OnEnable(){
        //PlayerMovement.OnDash += PerformDash;
    }

    void OnDisable(){
        //PlayerMovement.OnDash -= PerformDash;
    }

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        //_remainingDashes = _dashCount;
    }

    public void Init(UnitInfo info){
        _playerInfo = info;
        //_dashSFX = _playerInfo.DashSFX;
    }

    private void Update()
    {
        //ResetDashes();

        /*if (_playerInfo.isDashing)
        {
            _dashTime -= Time.deltaTime;
            if (_dashTime <= 0)  // Set dash flag to false when dashTime ends
            {
                _playerInfo.isDashing = false; 
            }
        }
    }


    public void PerformDash(Vector2 moveDirection)
    {
        // TO DO :
        // Don't forget to change this implementation when Pausing/Unpausing is added
        // Time.time WILL cause issues then
        if (_remainingDashes > 0)
        {
            _playerInfo.isDashing = true;
            //_dashTime = _abilityData.dashDuration;

            PlayPlayerSounds.PlayAudio(_dashSFX, .25f);
            //StartCoroutine(onDashTriggered?.Invoke(_playerInfo, _abilityData.dashDuration));
            _timeSinceLastDash = Time.time;

            Vector2 dashDirection = moveDirection.normalized;
            //_rb2D.velocity = dashDirection * _abilityData.dashSpeed;
            

            _remainingDashes--;
        }
    }

    // TO DO : Tweak the time for dash reset
    // Resets dash count when remainingDashes isn't at maximum
    // and that some time has passed since the player's last dash
    public void ResetDashes()
    {
        if( (_remainingDashes != _dashCount) &&  (1.5f < Time.time - _timeSinceLastDash) ){
            //Debug.Log("Time since last dash : " + (Time.time - timeSinceLastDash));
            _remainingDashes = _dashCount;
            PlayPlayerSounds.PlayAudio(_playerInfo.DashRecoverSFX);
        }
        
    }*/


}
