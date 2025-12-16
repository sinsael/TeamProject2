using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Stat : MonoBehaviour
{
    public StatSO defaultStatSetup;


    public SanityStat san = new SanityStat
    {
        maxSanity = new Stat(),
        SanityRegen = new Stat(),
        SanityAmount = new Stat()
    };

    public MoveStat move = new MoveStat
    {
        speed = new Stat(),
        jumpForce = new Stat()
    };


    public void Awake()
    {
        // 스탯 초기화
        if (defaultStatSetup != null)
        {
            san.maxSanity.SetBaseValue(defaultStatSetup.Sanity);
            san.SanityRegen.SetBaseValue(defaultStatSetup.RegenSanity);
            san.SanityAmount.SetBaseValue(defaultStatSetup.SanityAmount);

            move.speed.SetBaseValue(defaultStatSetup.speed);
            move.jumpForce.SetBaseValue(defaultStatSetup.jumpForce);
        }
    }

    // Sanity 관련 함수
    public float GetSanity()
    {
        return san.maxSanity.GetValue();
    }

    // Move 관련 함수
    public float GetSpeed()
    {
        return move.speed.GetValue();
    }

    // Jump 관련 함수
    public float GetJumpForce()
    {
        return move.jumpForce.GetValue();
    }

    // StatType에 해당하는 Stat 반환
    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxSanity : return san.maxSanity;
            case StatType.SanityRegen : return san.SanityRegen;
            case StatType.Speed : return move.speed;
            case StatType.JumpForce : return move.jumpForce;

            default: return null;
        }
    }


}
