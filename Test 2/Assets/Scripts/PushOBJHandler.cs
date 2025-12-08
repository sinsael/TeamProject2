using UnityEngine;
using UnityEngine.InputSystem;

public class PushOBJHandler : MonoBehaviour
{
    [Header("Push Block")]
    public float distance = 1f;
    public float yOffset = 0.2f;

    [Header("Push Move Slow")]
    [SerializeField] private float pushingSpeedMultiplier = 0.4f;

    [Header("Push Requirement")]
    [SerializeField] private bool requireItemToPush = false;
    [SerializeField] private ItemData requiredItemToPush;
    [SerializeField] private bool consumeItemOnBeginPush = false;

    private First_Player first;

    private GameObject pushingOBJ = null;
    private FixedJoint2D pushingJoint = null;
    private bool isPushing = false;

    public bool IsPushing;

    public void Init(First_Player p)
    {
        first = p;
    }

    public void PushingSystem()
    {
        PushingOBJ();
        PushingJumpBlock();
        PushingSlowMove();
    }

    private void PushingOBJ()
    {
        if (first.wall.IswallDetected && !first.ground.IsgroundDetected)
        {
        }
        if (!first.ground.IsgroundDetected && !first.wall.IswallDetected)
        {
        }
        if (!first.wall.IswallDetected && first.ground.IsgroundDetected)
        {
            if (first.rb.linearVelocityY > 0.1f || first.rb.linearVelocityY < -0.1f)
            {
                return;
            }
            if (first.inputSystem.InteractableHoldInput() && !isPushing)
            {
                StartGrabOBJ();
            }
            else if (!first.inputSystem.InteractableHoldInput() && isPushing)
            {
                ExitGrabOBJ();
            }
        }
    }

    private void StartGrabOBJ()
    {
        Physics2D.queriesStartInColliders = false;

        Vector2 origin = transform.position;
        origin.y += yOffset;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.right * transform.localScale.x,
            distance
        );

        if (hit.collider == null)
            return;

        if (!hit.collider.CompareTag("Push_OBJ"))
            return;

        if (requireItemToPush)
        {
            if (!Inventory.Instance.HasItem(requiredItemToPush))
                return;
        }

        pushingOBJ = hit.collider.gameObject;

        pushingJoint = pushingOBJ.GetComponent<FixedJoint2D>();
        if (pushingJoint != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            pushingJoint.connectedBody = rb;
            pushingJoint.enabled = true;

            if (requireItemToPush && consumeItemOnBeginPush)
                Inventory.Instance.RemoveItem(requiredItemToPush);
        }

        PushingOBJ pull = pushingOBJ.GetComponent<PushingOBJ>();
        if (pull != null)
            pull.beingPushed = true;

        isPushing = true;
    }

    private void ExitGrabOBJ()
    {
        if (pushingOBJ == null)
        {
            isPushing = false;
            pushingJoint = null;
            return;
        }

        if (pushingJoint == null)
            pushingJoint = pushingOBJ.GetComponent<FixedJoint2D>();

        if (pushingJoint != null)
        {
            pushingJoint.enabled = false;
            pushingJoint.connectedBody = null;
        }

        PushingOBJ pull = pushingOBJ.GetComponent<PushingOBJ>();
        if (pull != null)
            pull.beingPushed = false;

        pushingOBJ = null;
        pushingJoint = null;
        isPushing = false;
    }

    private void PushingJumpBlock()
    {
        if (!isPushing)
            return;

        if (first.inputSystem.JumpInput())
        {
            Vector2 v = first.rb.linearVelocity;

            if (v.y > 0f)
            {
                v.y = 0f;
                first.rb.linearVelocity = v;
            }
        }
    }

    private void PushingSlowMove()
    {
        if (!isPushing)
            return;

        Vector2 v = first.rb.linearVelocity;
        v.x *= pushingSpeedMultiplier;
        first.rb.linearVelocity = v;
    }

    public void PushingotherMoveBlock()
    {
        if (!isPushing) return;

        Vector2 input = first.inputSystem.moveInput;

        if (first.transform.localScale.x > 0f && input.x < 0f)
        {
            input.x = 0f;
        }
        else if (first.transform.localScale.x < 0f && input.x > 0f)
        {
            input.x = 0f;
        }

        first.inputSystem.moveInput = input;
    }

    private void OnDrawGizmosSelected()
    {
        if (first == null) first = GetComponent<First_Player>();
        if (first == null) return;

        Gizmos.color = Color.yellow;

        Vector3 origin = first.transform.position;
        origin.y += yOffset;

        Vector3 dir = Vector2.right * first.transform.localScale.x;
        Gizmos.DrawLine(origin, origin + dir * distance);
    }
}
