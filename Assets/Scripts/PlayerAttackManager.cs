using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttackManager : MonoBehaviour
{
    private List<PlayerAttack> activePlayerAttacks = new List<PlayerAttack>();
    private List<PlayerAttack> playerAttacksToUnsubscribe = new List<PlayerAttack>();

    [SerializeField] private PlayerAttack playerAttack_Prefab;

    public void SpawnPlayerAttack(GamePlayer player)
    {
        var newPlayerAttack = Instantiate(playerAttack_Prefab);
        Transform pos = null;
        bool currentSide = player.playerNo == 1 ? true : false;

        newPlayerAttack.transform.parent = pos;
        newPlayerAttack.transform.localPosition = Vector3.zero;

        newPlayerAttack.OnAwake(this, player.playerNo, currentSide);

        activePlayerAttacks.Add(newPlayerAttack);
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

    public void UnsubscribePlayerAttack(PlayerAttack attackInst)
    {
        playerAttacksToUnsubscribe.Add(attackInst);
    }
}