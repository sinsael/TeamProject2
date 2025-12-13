using UnityEngine;

public class PlayerWallCrushAnimEvent : MonoBehaviour
{
    private Interaction_BreakWall target;
    private bool hitFired;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void Begin(Interaction_BreakWall t)
    {
        target = t;
        hitFired = false;
    }

    public void WallCrushHit()
    {
        if (hitFired) return;
        hitFired = true;

        if (target != null)
        {
            target.HitWallFromAnimation();
        }
    }

    public void WallCrushEnd()
    {
        target = null;
        hitFired = false;

        if (player != null)
        {
            player.EndWallCrushFromAnim();
        }
    }
}
