using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticle : MonoBehaviour
{    void Start()
    {
        Destroy(gameObject, 0.5f);
    }
}
