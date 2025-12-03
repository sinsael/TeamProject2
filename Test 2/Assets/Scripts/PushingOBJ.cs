using UnityEngine;

// ===========================
// 밀고 싶은 오브젝트에 부착
// ===========================

public class PushingOBJ : MonoBehaviour
{
    public bool beingPushed;

    private float xPos;
    public Vector3 lastPos;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.12f;
    [SerializeField] private float groundCheckInset = 0.02f;
    [SerializeField] private float noGroundGraceTime = 0.08f;
    [SerializeField] private LayerMask groundLayer;

    private FixedJoint2D joint;
    private Rigidbody2D rb;
    private Collider2D col;

    private float noGroundTimer = 0f;
    private bool hasGround = false;

    private RigidbodyConstraints2D baseConstraints;

    private void Start()
    {
        joint = GetComponent<FixedJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb != null) baseConstraints = rb.constraints;

        xPos = transform.position.x;
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        hasGround = CheckGroundUnderBox();

        UpdateFreezeXByPushing();

        if (joint != null && joint.enabled)
        {
            if (!hasGround)
            {
                noGroundTimer += Time.fixedDeltaTime;

                if (noGroundTimer >= noGroundGraceTime)
                {
                    joint.enabled = false;
                    beingPushed = false;
                }
            }
            else
            {
                noGroundTimer = 0f;
            }
        }

        if (!beingPushed)
        {
            if (hasGround)
            {
                transform.position = new Vector3(
                    xPos,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
        else
        {
            xPos = transform.position.x;
            lastPos = transform.position;
        }
    }

    private void UpdateFreezeXByPushing()
    {
        if (rb == null) return;

        if (beingPushed)
        {
            rb.constraints = rb.constraints & ~RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            rb.constraints = baseConstraints;
        }
    }

    private bool CheckGroundUnderBox()
    {
        if (col == null) return false;

        Bounds b = col.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y + groundCheckInset);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null) return;

        Bounds b = col.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y + groundCheckInset);
        Vector2 end = origin + Vector2.down * groundCheckDistance;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origin, end);
    }
}
