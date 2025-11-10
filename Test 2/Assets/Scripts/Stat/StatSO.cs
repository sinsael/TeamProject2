using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EntityData", menuName = "Stat/StatData")]
public class StatSO : ScriptableObject
{
    public float speed;
    public float jumpForce;
    public float Sanity;
    public float RegenSanity;
    public float SanityAmount;
}
