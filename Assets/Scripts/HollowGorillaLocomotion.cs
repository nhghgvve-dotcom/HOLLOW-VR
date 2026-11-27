using UnityEngine;

/// <summary>
/// Early HOLLOW arm-powered locomotion prototype.
/// Attach to the Rigidbody player root. Assign tracked controller/hand transforms
/// from the XR Origin. Hands are expected to move with real OpenXR controller tracking.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HollowGorillaLocomotion : MonoBehaviour
{
    [Header("Tracked hands")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [Header("Locomotion")]
    [SerializeField, Min(0.02f)] private float handRadius = 0.08f;
    [SerializeField, Min(0f)] private float maxPushSpeed = 7.5f;
    [SerializeField, Min(0f)] private float velocityDamping = 0.98f;
    [SerializeField] private LayerMask locomotionMask = ~0;

    private Rigidbody body;
    private Vector3 lastLeftPosition;
    private Vector3 lastRightPosition;
    private Vector3 leftVelocity;
    private Vector3 rightVelocity;
    private bool initialized;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = true;
        body.constraints |= RigidbodyConstraints.FreezeRotation;
        body.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        ResetHandHistory();
    }

    private void FixedUpdate()
    {
        if (leftHand == null || rightHand == null)
            return;

        if (!initialized)
        {
            ResetHandHistory();
            return;
        }

        Vector3 leftNow = leftHand.position;
        Vector3 rightNow = rightHand.position;

        Vector3 leftDelta = lastLeftPosition - leftNow;
        Vector3 rightDelta = lastRightPosition - rightNow;

        bool leftBlocked = HandOverlapsSurface(leftNow);
        bool rightBlocked = HandOverlapsSurface(rightNow);

        Vector3 push = Vector3.zero;
        if (leftBlocked)
            push += leftDelta;
        if (rightBlocked)
            push += rightDelta;

        // Move the player opposite the hand's attempted movement when the hand
        // is pressed into a solid surface, producing arm-powered locomotion.
        if (push.sqrMagnitude > 0f)
        {
            Vector3 nextPosition = body.position + push;
            body.MovePosition(nextPosition);

            Vector3 inferredVelocity = push / Time.fixedDeltaTime;
            if (inferredVelocity.magnitude > maxPushSpeed)
                inferredVelocity = inferredVelocity.normalized * maxPushSpeed;

            body.linearVelocity = new Vector3(
                inferredVelocity.x,
                body.linearVelocity.y,
                inferredVelocity.z
            );
        }
        else
        {
            Vector3 v = body.linearVelocity;
            body.linearVelocity = new Vector3(
                v.x * velocityDamping,
                v.y,
                v.z * velocityDamping
            );
        }

        lastLeftPosition = leftNow;
        lastRightPosition = rightNow;
    }

    private bool HandOverlapsSurface(Vector3 handPosition)
    {
        Collider[] hits = Physics.OverlapSphere(
            handPosition,
            handRadius,
            locomotionMask,
            QueryTriggerInteraction.Ignore
        );

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody == body)
                continue;

            // Ignore trigger volumes and the player's own colliders.
            if (!hit.isTrigger && !hit.transform.IsChildOf(transform))
                return true;
        }

        return false;
    }

    /// <summary>Call after teleporting/repositioning the rig to avoid a movement spike.</summary>
    public void ResetHandHistory()
    {
        if (leftHand == null || rightHand == null)
        {
            initialized = false;
            return;
        }

        lastLeftPosition = leftHand.position;
        lastRightPosition = rightHand.position;
        initialized = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (leftHand != null)
            Gizmos.DrawWireSphere(leftHand.position, handRadius);
        if (rightHand != null)
            Gizmos.DrawWireSphere(rightHand.position, handRadius);
    }
}
