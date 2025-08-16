using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BaseAudioSliderVisuals : MonoBehaviour
{
    public Image selectionIndicator;
    public Sprite[] volumeBarSprites;
    public Image volumeBarImage;

    public virtual void UpdateVisuals() { }

}
