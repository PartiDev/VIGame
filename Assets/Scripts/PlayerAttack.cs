using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GamePlayer attackingPlayer { get; private set; }
    public GamePlayer attackedPlayer { get; private set; }
     public bool currentPlayerSide { get; private set; }
    public bool inParryWindow { get; private set; }
    public bool isParried { get; private set; }

    public PlayerAttackMode thisAttackMode;

    public int attackAmount {get; private set;}

    private PlayerAttackManager playerAttackManager;

    public void OnAwake(PlayerAttackManager _attackManager, GamePlayer _attackingPlayer, GamePlayer _opposingPlayer, bool isOnBeat = false)
    {
        inParryWindow = false;

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

    public void ActivateParryWindow()
    {
        inParryWindow = true;
    }

    public void ParryAttack(AttackType type)
    {
        if (type == thisAttackMode.counteringAttackType)
        {
            //Succeeds!
            inParryWindow = false;
            isParried = true;
        }
    }
}