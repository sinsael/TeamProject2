using UnityEngine;

public class Interaction_Vent : Interaction_Obj
{
    private bool used;

    public override void OnInteract(PlayerInputHandler playerInput)
    {
        base.OnInteract(playerInput);

        if (used)
            return;

        used = true;

        GameManager.Instance.ChangeGameState(GameState.GameClear);
    }
}
