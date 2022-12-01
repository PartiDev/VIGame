using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public GamePlayer attackingPlayer;
    [HideInInspector] public GamePlayer attackedPlayer;
    [HideInInspector] public bool currentPlayerSide;
    public PlayerAttackMode thisAttackMode;
    
    public int attackAmount {get; private set;}

    private PlayerAttackManager playerAttackManager;

    private bool phase = false;

    public void OnAwake(PlayerAttackManager _attackManager, GamePlayer _attackingPlayer, GamePlayer _opposingPlayer, bool isOnBeat = false)
    {
        playerAttackManager = _attackManager;
        attackingPlayer = _attackingPlayer;
        attackedPlayer = _opposingPlayer;
        
        attackAmount = isOnBeat ? thisAttackMode.damageAmount + thisAttackMode.damageBoostAmount : thisAttackMode.damageAmount;

        bool leftRight = false;
        AudioClip thisAudio = thisAttackMode.attackSound_Major;
        if (attackingPlayer.playerNo == 1)
        {
            leftRight = true;
        }

        AudioManager.Instance.PlaySFXDirectional(thisAudio, leftRight, 0.5f, 1);
    }

    public void ParryAttack(int _playerNo)
    {

    }
}