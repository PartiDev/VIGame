using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [HideInInspector] public int currentPos;
    [HideInInspector] public int playerNo;
    [HideInInspector] public bool currentPlayerSide;
    private bool onFirstSide = true;
    public bool moving = false;
    [HideInInspector] public PlayerAttackSizes playerAttackSize;

    [SerializeField] private int attackAmount_Small;
    [SerializeField] private int attackAmount_Medium;
    [SerializeField] private int attackAmount_Big;
    private int currentAttackAmount;

    [SerializeField] private Sprite[] sizeTypes;
    [SerializeField] private SpriteRenderer spriteToChange;

    [SerializeField] private AudioClip[] playerPianoAttackSounds;
    [SerializeField] private AudioClip[] playerGuitarAttackSounds;

    private PlayerAttackManager playerAttackManager;

    public void OnAwake(PlayerAttackManager _attackManager, int _playerNo, bool _currentSide)
    {
        playerAttackManager = _attackManager;
        playerNo = _playerNo;
        currentPlayerSide = _currentSide;

        switch (currentPos)
        {
            case (0):
                playerAttackSize = PlayerAttackSizes.Small;
                currentAttackAmount = attackAmount_Small;
                break;
            case (1):
                playerAttackSize = PlayerAttackSizes.Medium;
                currentAttackAmount = attackAmount_Medium;
                break;
            case (2):
                playerAttackSize = PlayerAttackSizes.Big;
                currentAttackAmount = attackAmount_Big;
                break;
        }

        spriteToChange.sprite = sizeTypes[currentPos];

        bool leftRight = false;
        AudioClip thisAudio = null;
        if (currentPlayerSide == true)
        {
            thisAudio = playerPianoAttackSounds[currentPos];
            leftRight = true;
        }
        else thisAudio = playerGuitarAttackSounds[currentPos];

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

    //I got tired and lazy around this part, the arguments should be handled differently lol
    public void MoveAttackForward(Transform[] leftPositions, Transform[] rightPositions)
    {
        if (moving)
        {
            if (onFirstSide)
            {
                if (currentPos + 1 > 2)
                {
                    onFirstSide = false;
                    currentPlayerSide = !currentPlayerSide;
                }
                else
                {
                    currentPos++;
                }
            }
            else
            {
                if (currentPos - 1 < 0)
                {
                    playerAttackManager.UnsubscribePlayerAttack(this);
                    GameObject.Destroy(this.gameObject);
                }
                else
                {
                    currentPos--;
                }
            }
        }

        if (currentPlayerSide)
        {
            this.transform.parent = leftPositions[currentPos];

            if (leftPositions[currentPos].GetComponentInChildren<GamePlayer>())
            {
                var player = leftPositions[currentPos].GetComponentInChildren<GamePlayer>();
                if (player.playerNo != playerNo)
                {
                    player.QueuePlayerAttack(this);
                    playerAttackManager.UnsubscribePlayerAttack(this);
                    moving = false;
                }
            }
        }
        else
        {
            this.transform.parent = rightPositions[currentPos];

            if (rightPositions[currentPos].GetComponentInChildren<GamePlayer>())
            {
                var player = rightPositions[currentPos].GetComponentInChildren<GamePlayer>();
                if (player.playerNo != playerNo)
                {
                    player.QueuePlayerAttack(this);
                    playerAttackManager.UnsubscribePlayerAttack(this);
                    moving = false;
                }
            }
        }

        this.transform.localPosition = Vector3.zero;
    }
}

public enum PlayerAttackSizes { Big, Medium, Small}