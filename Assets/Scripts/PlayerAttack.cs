using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public int playerNo;
    [HideInInspector] public bool currentPlayerSide;
    public PlayerAttackMode thisAttackMode;

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

        AudioManager.Instance.PlaySFXDirectional(thisAudio, leftRight, 0.5f, 1);
    }

    public void ParryAttack(int _playerNo)
    {
        playerNo = _playerNo;
        currentPlayerSide = !currentPlayerSide;
    }
}