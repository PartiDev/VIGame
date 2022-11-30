using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public int playerNo;
    [HideInInspector] public bool currentPlayerSide;
    private bool onFirstSide = true;
    public bool moving = false;

    private int currentAttackAmount;

    [SerializeField] private AudioClip piano_AttackSound_Major;
    [SerializeField] private AudioClip guitar_AttackSound_Major;
    [SerializeField] private AudioClip flute_AttackSound_Major;

    private PlayerAttackManager playerAttackManager;

    public void OnAwake(PlayerAttackManager _attackManager, int _playerNo, bool _currentSide)
    {
        playerAttackManager = _attackManager;
        playerNo = _playerNo;
        currentPlayerSide = _currentSide;

        bool leftRight = false;
        AudioClip thisAudio = null;
        if (currentPlayerSide == true)
        {
            leftRight = true;
        }

        moving = true;
        AudioManager.Instance.PlaySFXDirectional(thisAudio, leftRight, 0.5f, 1);
    }

    public void ParryAttack(int _playerNo)
    {
        playerNo = _playerNo;
        currentPlayerSide = !currentPlayerSide;
        onFirstSide = true;
        moving = true;
    }
}

public enum PlayerAttackMode { Piano, Flute, Guitar }
public enum ChordType { Major, Minor }