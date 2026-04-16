using UnityEngine;
using UnityEngine.Events;

public class AnimationEventStateBehaviour : StateMachineBehaviour {
    public string eventName;
    [Range(0f, 1f)] public float triggerTime;
    public bool isLooping;
    bool hasTriggered;
    AnimationEventReceiver receiver;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        hasTriggered = false;
        receiver = animator.GetComponent<AnimationEventReceiver>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float currentTime = stateInfo.normalizedTime % 1f;

        // Looping animation looped back and we are not past the trigger threshold
        // Reset the hasTriggered bool.
        if (isLooping && hasTriggered && currentTime <= triggerTime)
            hasTriggered = false;

        if (!hasTriggered && currentTime >= triggerTime)
        {
            NotifyReceiver(animator);
            hasTriggered = true;
        }
    }

    void NotifyReceiver(Animator animator) {
        if (receiver != null) {
            receiver.OnAnimationEventTriggered(eventName);
            //Debug.Log($"Notified {receiver} with the event {eventName}.");
        }
    }
}
