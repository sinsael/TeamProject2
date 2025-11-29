using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Second_Player : Player
{
    protected override void Start()
    {
        base.Start();

        First_Player p1 = FindAnyObjectByType<First_Player>();
        otherPlayer = p1.GetComponent<First_Player>();
    }
}
