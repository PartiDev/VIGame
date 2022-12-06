using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private GamePlayer Player1;
        [SerializeField] private GamePlayer Player2;

        [SerializeField] private PlayerAttackManager attackManager;
        [SerializeField] private Metronome metronome;
        private bool gameOver = false;
        [SerializeField] private float tickMoveWindow = 0.05f;

        [SerializeField] private PlayerAttack[] attackModePrefabs;

        [SerializeField] private Animator gameOverPanel;
        [SerializeField] private TextMeshProUGUI playerLost;

        private void Awake()
        {
            metronome.OnMetronomeTick += AllowPlayerTick;
            Player1.OnPlayerDeath += GameOver;
            Player2.OnPlayerDeath += GameOver;
        }

        public void AllowPlayerTick()
        {
            StartCoroutine(OpenTickWindow());
        }

        public void Reload()
        {
            SceneManager.LoadScene(0);
        }

        private IEnumerator OpenTickWindow()
        {
            Player2.metronomeTickActive = true;
            Player1.metronomeTickActive = true;
            yield return new WaitForSeconds(tickMoveWindow);
            Player2.metronomeTickActive = false;
            Player1.metronomeTickActive = false;
        }

        //Could be generalised to prevent duplication
        public void FixedUpdate()
        {
            //Restart
            if (Input.GetKey(KeyCode.R))
            {
                Reload();
            }

            if (!gameOver) {

            //Player 1 input types
            if (Player1.canAttack)
            {
                if (Input.GetKey(Player1.Attack))
                {
                    PlayerAttack currentPlayerAttack = null;
                    if (Input.GetKey(Player1.Piano_Mode))
                    {
                        currentPlayerAttack = attackModePrefabs[0];
                    }

                    if (Input.GetKey(Player1.Guitar_Mode))
                    {
                        currentPlayerAttack = attackModePrefabs[1];
                    }

                    if (Input.GetKey(Player1.Flute_Mode))
                    {
                        currentPlayerAttack = attackModePrefabs[2];
                    }

                    if (currentPlayerAttack != null)
                    {
                        attackManager.PlayerAttack(Player1, Player2, currentPlayerAttack);
                        Player1.metronomeTickActive = false;
                    }
                }

                if (Player1.canParry)
                {
                    if (Input.GetKey(Player1.Parry))
                    {
                        if (Player1.incomingAttack && Player1.incomingAttack.inParryWindow)
                        {
                            AttackType? attackType = null;
                            if (Input.GetKey(Player1.Piano_Mode))
                            {
                                attackType = AttackType.Piano;
                            }

                            if (Input.GetKey(Player1.Guitar_Mode))
                            {
                                attackType = AttackType.Guitar;
                            }

                            if (Input.GetKey(Player1.Flute_Mode))
                            {
                                attackType = AttackType.Flute;
                            }

                            if (attackType != null)
                            {
                                attackManager.TryParryAttack(Player1, attackType.Value);
                            }
                        }
                    }
                }
            }

                //Player 2 input types
                if (Player2.canAttack)
                {
                    if (Input.GetKey(Player2.Attack))
                    {
                        PlayerAttack currentPlayerAttack = null;
                        if (Input.GetKey(Player2.Piano_Mode))
                        {
                            currentPlayerAttack = attackModePrefabs[0];
                        }

                        if (Input.GetKey(Player2.Guitar_Mode))
                        {
                            currentPlayerAttack = attackModePrefabs[1];
                        }

                        if (Input.GetKey(Player2.Flute_Mode))
                        {
                            currentPlayerAttack = attackModePrefabs[2];
                        }

                        if (currentPlayerAttack != null)
                        {
                            attackManager.PlayerAttack(Player2, Player1, currentPlayerAttack);
                            Player2.metronomeTickActive = false;
                        }
                    }

                    if (Player2.canParry)
                    {
                        if (Input.GetKey(Player2.Parry))
                        {
                            if (Player2.incomingAttack && Player2.incomingAttack.inParryWindow)
                            {
                                AttackType? attackType = null;
                                if (Input.GetKey(Player2.Piano_Mode))
                                {
                                    attackType = AttackType.Piano;
                                }

                                if (Input.GetKey(Player2.Guitar_Mode))
                                {
                                    attackType = AttackType.Guitar;
                                }

                                if (Input.GetKey(Player2.Flute_Mode))
                                {
                                    attackType = AttackType.Flute;
                                }

                                if (attackType != null)
                                {
                                    attackManager.TryParryAttack(Player2, attackType.Value);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GameOver(GamePlayer player)
        {
            //Should be moved to a Canvas manager
            gameOver = true;
            gameOverPanel.SetTrigger("FadeIn");
            playerLost.text = "Player " + player.playerNo.ToString() + " lost the game.";
        }
    }
}