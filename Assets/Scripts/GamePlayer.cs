using System.Collections;
using UnityEngine;
public class GamePlayer : MonoBehaviour
{
    public int playerNo;
    [SerializeField] private PlayerTypes thisPlayerType;

    public KeyCode Guitar_Mode;
    public KeyCode Piano_Mode;
    public KeyCode Flute_Mode;
    public KeyCode Attack;
    public KeyCode Parry;

    public bool metronomeTickActive = true;
    public bool movementOnCooldown = false;

    [SerializeField] private int totalHealth = 100;
    private int currentHealth;
    [SerializeField] private AudioClip hurtSound;
    public AudioClip parrySound;

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
        AudioManager.Instance.PlaySFXDirectional(hurtSound, leftRight, volume);
        GameObject.Destroy(incomingAttack.gameObject);

        Debug.Log(currentHealth);
    }
}

public enum PlayerTypes { Piano, Guitar }