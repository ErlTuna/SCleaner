using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Hat Knock Down Behaivour", menuName = "ScriptableObjects/Component Configs/Hat/Shrink And Disappear Behaviour")]
public class HatShrinkAndDisappear : HatKnockDownBehaviour
{
    public float ShrinkDuration;
    public float InitialDelay;

    public override void Execute(GameObject hat)
    {
        MonoBehaviour runner = hat.GetComponent<MonoBehaviour>();
        Rigidbody2D rb2D = hat.GetComponent<Rigidbody2D>();
        Transform transform = hat.transform;

        if (rb2D == null) return;
        if (runner == null) return;

        runner.StartCoroutine(FadeAndDestroy(hat, rb2D, transform));
    }



    IEnumerator FadeAndDestroy(GameObject hat, Rigidbody2D rb2D, Transform transform)
    {
        if (rb2D == null) yield break;

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rb2D.velocity.magnitude < 0.05f);
        yield return new WaitForSeconds(InitialDelay);

        Vector3 startScale = transform.localScale;
        float t = 0f;

        while (t < ShrinkDuration)
        {
            t += Time.deltaTime;
            float progress = t / ShrinkDuration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            yield return null;
        }

        Destroy(hat);
    }
}
