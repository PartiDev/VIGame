using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerAttackMode
{
    [SerializeField] public AttackType attackType;
    [SerializeField] public AttackType counteringAttackType;
    [SerializeField] public float leadUpTime;
    [SerializeField] private int damageAmount;
    public AudioClip attackSound_Major;
    public AudioClip hurtSound;
}

public enum AttackType { Piano, Flute, Guitar }
public enum ChordType { Major, Minor }