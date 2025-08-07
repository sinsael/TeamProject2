using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet playerInput;

    protected PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        playerInput = player.input;
    }
}
    