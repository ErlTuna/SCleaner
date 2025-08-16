using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWeaponSprite : MonoBehaviour
{
    bool isFlipped = false;
    void Update()
    {
        float rotationZ = transform.rotation.eulerAngles.z;

        if(!isFlipped && 90f < rotationZ && rotationZ < 270f){
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            isFlipped = true;
        }
        else if(isFlipped && (0f < rotationZ && rotationZ < 90f || 270f < rotationZ && rotationZ < 360f)){
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            isFlipped = false;
        }
    }
}
