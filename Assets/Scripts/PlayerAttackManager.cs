using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private float parryWindow;
    [SerializeField] private AudioClip[] parrySFX;

    public void PlayerAttack(GamePlayer attackingPlayer, GamePlayer opposingPlayer, PlayerAttack attackPrefab)
    {
        var newPlayerAttack = Instantiate(attackPrefab);
        attackingPlayer.movementOnCooldown = true;

        switch (attackPrefab.thisAttackMode.attackType)
        {
            case (AttackType.Piano):
                attackingPlayer.playerAnim.SetTrigger("Attack_Piano");
                break;
            case (AttackType.Guitar):
                attackingPlayer.playerAnim.SetTrigger("Attack_Guitar");
                break;
            case (AttackType.Flute):
                attackingPlayer.playerAnim.SetTrigger("Attack_Flute");
                break;
        }

        newPlayerAttack.transform.parent = attackingPlayer.transform;
        newPlayerAttack.transform.localPosition = Vector3.zero;

        newPlayerAttack.OnAwake(this, attackingPlayer, opposingPlayer, attackingPlayer.metronomeTickActive);

        attackingPlayer.activeAttack = newPlayerAttack;
        opposingPlayer.incomingAttack = newPlayerAttack;

        StartCoroutine(WaitForAttackWindUp(newPlayerAttack));
    }

    private IEnumerator WaitForAttackWindUp(PlayerAttack attack){
        yield return new WaitForSeconds(attack.thisAttackMode.leadUpTime);

        attack.ActivateParryWindow();

        yield return new WaitForSeconds(parryWindow);

        CheckForAttackInfluence(attack);
    }

    private void CheckForAttackInfluence(PlayerAttack attack)
    {
        if (!attack.isParried)
        {
            //Success
            switch (attack.thisAttackMode.attackType)
            {
                case (AttackType.Piano):
                    attack.attackingPlayer.playerAnim.SetTrigger("Piano_Success");
                    break;
                case (AttackType.Guitar):
                    attack.attackingPlayer.playerAnim.SetTrigger("Guitar_Success");
                    break;
                case (AttackType.Flute):
                    attack.attackingPlayer.playerAnim.SetTrigger("Flute_Success");
                    break;
            }

            attack.attackedPlayer.DamagePlayer();
        }
        else
        {
            switch (attack.thisAttackMode.attackType)
            {
                case (AttackType.Piano):
                    attack.attackingPlayer.playerAnim.SetTrigger("Piano_Fail");
                    break;
                case (AttackType.Guitar):
                    attack.attackingPlayer.playerAnim.SetTrigger("Guitar_Fail");
                    break;
                case (AttackType.Flute):
                    attack.attackingPlayer.playerAnim.SetTrigger("Flute_Fail");
                    break;
            }

            attack.attackingPlayer.DamagePlayerParry();

            attack.attackingPlayer.movementOnCooldown = false;
            attack.attackedPlayer.movementOnCooldown = false;
        }

        DestroyAttack(attack);
    }

    private void DestroyAttack(PlayerAttack attack){
        attack.attackingPlayer.activeAttack = null;
        attack.attackedPlayer.incomingAttack = null;
        GameObject.Destroy(attack.gameObject);
    }

    public void TryParryAttack(GamePlayer player, AttackType type)
    {
        player.movementOnCooldown = true;
        bool leftRight = player.playerNo == 1 ? true : false;

        switch (type)
        {
            case (AttackType.Piano):
                player.playerAnim.SetTrigger("Piano_Parry");
                AudioManager.Instance.PlaySFXDirectional(parrySFX[0], leftRight, 0.5f);
                break;
            case (AttackType.Guitar):
                player.playerAnim.SetTrigger("Guitar_Parry");
                AudioManager.Instance.PlaySFXDirectional(parrySFX[1], leftRight, 0.5f);
                break;
            case (AttackType.Flute):
                player.playerAnim.SetTrigger("Flute_Parry");
                AudioManager.Instance.PlaySFXDirectional(parrySFX[2], leftRight, 0.5f);
                break;
        }

        player.incomingAttack.ParryAttack(type);
    }
}