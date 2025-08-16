using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaseAudioSliderLogic : MonoBehaviour
{
    public Slider slider;
    public float currentValue;
    public bool suppressEvents = false;

    public virtual void AdjustVolume(float value) { }
    public virtual void Setup() { }
    public virtual void UpdateSliderLogic(){  }
}
