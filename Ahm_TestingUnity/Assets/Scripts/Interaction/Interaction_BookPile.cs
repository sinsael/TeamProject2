using UnityEngine;

public class Interaction_BookPile : Interaction_Obj
{
    private bool isUsed = false;

    public override void OnInteract(PlayerInputHandler interactor)
    {
        if (interactor == null)
            return;

        if (isUsed)
            return;

        isUsed = true;

        Interaction_BreakWall.UnlockWallGimmickByBookPile();
        Debug.Log("책 더미를 조사했다. 벽 기믹이 활성화되었다.");
    }
}
