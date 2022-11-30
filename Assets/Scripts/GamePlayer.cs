using System.Collections;
using UnityEngine;
public class GamePlayer : MonoBehaviour
{
    public int playerNo;
    [SerializeField] private PlayerTypes thisPlayerType;

    public KeyCode moveKey_Left;
    public KeyCode moveKey_Right;
    public KeyCode attackKey;
    public KeyCode parryKey;

    public int currentPosLevel = 1;
    public bool tickInputActive = true;

    [SerializeField] private int totalHealth = 100;
    private int currentHealth;
    [SerializeField] private int smallAttackDamage;
    [SerializeField] private int mediumAttackDamage;
    [SerializeField] private int bigAttackDamage;
    [SerializeField] private AudioClip hurtSound;
    public AudioClip parrySound;

    [HideInInspector] public PlayerAttack incomingAttack;

    private void Awake()
    {
        currentHealth = totalHealth;
    }

    public void QueuePlayerAttack(PlayerAttack attack)
    {
        incomingAttack = attack;
    }

    public void DamagePlayer()
    {
        if (incomingAttack == null) return;

        var attackSize = incomingAttack.playerAttackSize;
        var volume = 0.3f;
        if (attackSize == PlayerAttackSizes.Small)
        {
            currentHealth -= smallAttackDamage;
            volume -= 0.1f;
        } else if (attackSize == PlayerAttackSizes.Medium)
        {
            currentHealth -= mediumAttackDamage;
        } else if (attackSize == PlayerAttackSizes.Big)
        {
            currentHealth -= bigAttackDamage;
            volume += 0.1f;
        }

        bool leftRight = playerNo == 1 ? true : false;
        AudioManager.Instance.PlaySFXDirectional(hurtSound, leftRight, volume);
        GameObject.Destroy(incomingAttack.gameObject);

        Debug.Log(currentHealth);
    }
}

public enum PlayerTypes { Piano, Guitar }