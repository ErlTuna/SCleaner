using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DissolveVFX : MonoBehaviour
{
    [SerializeField] Animator _animator;

    void Start()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _animator.SetTrigger("DissolveTrigger");
        StartCoroutine(WaitForDissolve());
    }

    private IEnumerator WaitForDissolve()
    {
        yield return null;
        float length = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        Destroy(gameObject);
    }
}
