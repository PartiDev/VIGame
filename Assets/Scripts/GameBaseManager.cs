using System.Collections;
using UnityEngine;

public class GameBaseManager : MonoBehaviour
{
    public Transform[] L_positions;
    public Transform[] R_positions;

    [SerializeField] private AudioClip moveSound;

    public void MovePlayerToPos(GamePlayer player, int posLevel)
    {
        switch (player.playerNo)
        {
            case (1):
                player.transform.parent = L_positions[posLevel];
            break;
            case (2):
                player.transform.parent = R_positions[posLevel];
            break;
        }

        player.transform.localPosition = Vector3.zero;
        player.currentPosLevel = posLevel;

        if (player.currentPosLevel > 2) player.currentPosLevel = 2;
        if (player.currentPosLevel < 0) player.currentPosLevel = 0;

        PlayMoveSound(player);
    }

    public void PlayMoveSound(GamePlayer player)
    {
        var leftRight = player.playerNo == 1 ? true : false;

        float pitch = 1;

        if (player.currentPosLevel == 1)
        {
            pitch += 0.1f;
        } else if (player.currentPosLevel == 2)
        {
            pitch += 0.2f;
        }

        AudioManager.Instance.PlaySFXDirectional(moveSound, leftRight, 0.25f, pitch);
    }
}