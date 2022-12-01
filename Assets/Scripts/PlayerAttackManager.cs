using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttackManager : MonoBehaviour
{
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
        CheckForAttackInfluence(attack);
    }

    private void CheckForAttackInfluence(PlayerAttack attack)
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

        attack.attackingPlayer.movementOnCooldown = false;
        DestroyAttack(attack);
    }

    private void DestroyAttack(PlayerAttack attack){
        attack.attackingPlayer.activeAttack = null;
        attack.attackedPlayer.incomingAttack = null;
        GameObject.Destroy(attack.gameObject);
    }

    public void ParryAttack(GamePlayer player, PlayerAttack attack)
    {
        player.incomingAttack = null;
        GameObject.Destroy(attack.gameObject);

        bool leftRight = player.playerNo == 1 ? true : false;

        //AudioManager.Instance.PlaySFXDirectional(player.parrySound, leftRight, 0.4f);

        attack.ParryAttack(player.playerNo);
        //activePlayerAttacks.Add(attack);
    }
}