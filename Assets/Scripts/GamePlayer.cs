using System.Collections;
using UnityEngine;
public class GamePlayer : MonoBehaviour
{
    public int playerNo;

    public KeyCode Guitar_Mode;
    public KeyCode Piano_Mode;
    public KeyCode Flute_Mode;
    public KeyCode Attack;
    public KeyCode Parry;

    public bool metronomeTickActive = false;
    public bool movementOnCooldown = false;

    [SerializeField] private int totalHealth = 100;
    private int currentHealth;

    [HideInInspector] public PlayerAttack incomingAttack;

    [HideInInspector] public PlayerAttackMode currentAttackMode;

    private void Awake()
    {
        currentHealth = totalHealth;
    }

    public void DamagePlayer()
    {
        if (incomingAttack == null) return;

        var volume = 0.3f;

        bool leftRight = playerNo == 1 ? true : false;
        AudioManager.Instance.PlaySFXDirectional(incomingAttack.thisAttackMode.attackSound_Major, leftRight, volume);
        GameObject.Destroy(incomingAttack.gameObject);

        Debug.Log(currentHealth);
    }
}