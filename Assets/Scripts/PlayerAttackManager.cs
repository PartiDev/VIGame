using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttackManager : MonoBehaviour
{
    private List<PlayerAttack> activePlayerAttacks = new List<PlayerAttack>();
    private List<PlayerAttack> playerAttacksToUnsubscribe = new List<PlayerAttack>();

    [SerializeField] private GameBaseManager gameBaseManager;
    [SerializeField] private PlayerAttack playerAttack_Prefab;

    public void SpawnPlayerAttack(GamePlayer player, int position)
    {
        var newPlayerAttack = Instantiate(playerAttack_Prefab);
        Transform pos = null;
        bool currentSide = player.playerNo == 1 ? true : false;

        if (player.playerNo == 1)
        {
            pos = gameBaseManager.L_positions[position];
        } else pos = gameBaseManager.R_positions[position];

        newPlayerAttack.currentPos = position;
        newPlayerAttack.transform.parent = pos;
        newPlayerAttack.transform.localPosition = Vector3.zero;

        newPlayerAttack.OnAwake(this, player.playerNo, currentSide);

        activePlayerAttacks.Add(newPlayerAttack);
    }

    public void MovePlayerAttacks()
    {
        foreach (PlayerAttack attack in activePlayerAttacks)
        {
            attack.MoveAttackForward(gameBaseManager.L_positions, gameBaseManager.R_positions);
        }

        //To prevent list moderation during runtime
        foreach (PlayerAttack attack in playerAttacksToUnsubscribe)
        {
            activePlayerAttacks.Remove(attack);
        }
    }

    public void ParryAttack(GamePlayer player, PlayerAttack attack)
    {
        player.incomingAttack = null;
        GameObject.Destroy(attack.gameObject);

        bool leftRight = player.playerNo == 1 ? true : false;

        AudioManager.Instance.PlaySFXDirectional(player.parrySound, leftRight, 0.4f);

        attack.ParryAttack(player.playerNo);
        //activePlayerAttacks.Add(attack);
    }

    public void UnsubscribePlayerAttack(PlayerAttack attackInst)
    {
        playerAttacksToUnsubscribe.Add(attackInst);
    }
}