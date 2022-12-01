using System.Collections;
using UnityEngine;

public class PlayerAttackMode : MonoBehaviour
{
    [HideInInspector] public AttackType attackType;
    [HideInInspector] public AttackType counteringAttackType;
    [HideInInspector] public float leadUpTime;
    [SerializeField] private int damageAmount;
    public AudioClip attackSound_Major;
    public AudioClip hurtSound;
}

public enum AttackType { Piano, Flute, Guitar }
public enum ChordType { Major, Minor }