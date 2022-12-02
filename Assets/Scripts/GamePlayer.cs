using System.Collections;
using UnityEngine;
public class GamePlayer : MonoBehaviour
{
    public int playerNo;
    public Animator playerAnim;
    public KeyCode Guitar_Mode;
    public KeyCode Piano_Mode;
    public KeyCode Flute_Mode;
    public KeyCode Attack;
    public KeyCode Parry;

    public bool metronomeTickActive = false;
    public bool movementOnCooldown = false;

    [SerializeField] private int totalHealth = 100;
    private int currentHealth;

    [HideInInspector] public PlayerAttack activeAttack;
    [HideInInspector] public PlayerAttack incomingAttack;

    private void Awake()
    {
        currentHealth = totalHealth;
    }

    public void DamagePlayer()
    {
        if (incomingAttack == null) return;

        var volume = 0.3f;

        bool leftRight = playerNo == 1 ? true : false;
        AudioManager.Instance.PlaySFXDirectional(incomingAttack.thisAttackMode.hurtSound, leftRight, volume);
        this.totalHealth -= incomingAttack.attackAmount;
        GameObject.Destroy(incomingAttack.gameObject);
    }

    public void DamagePlayerParry()
    {
        if (activeAttack == null) return;

        var volume = 0.3f;

        bool leftRight = playerNo == 1 ? true : false;
        AudioManager.Instance.PlaySFXDirectional(activeAttack.thisAttackMode.hurtSound, leftRight, volume);
        this.totalHealth -= activeAttack.attackAmount * 2;
        GameObject.Destroy(activeAttack.gameObject);
    }
}