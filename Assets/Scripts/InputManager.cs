using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private GamePlayer Player1;
        [SerializeField] private GamePlayer Player2;

        [SerializeField] private bool turnImplementation;
        private bool currentTurn = true;

        [SerializeField] private GameBaseManager gameBaseManager;
        [SerializeField] private PlayerAttackManager attackManager;
        [SerializeField] private Metronome metronome;
        [SerializeField] private float tickMoveWindow = 0.1f;

        private void Awake()
        {
            metronome.OnMetronomeTick += AllowPlayerTick;
            metronome.OnMetronomeTick += attackManager.MovePlayerAttacks;
        }

        public void AllowPlayerTick()
        {
            StartCoroutine(OpenTickWindow());
        }

        private IEnumerator OpenTickWindow()
        {
            Player2.tickInputActive = true;
            Player1.tickInputActive = true;
            yield return new WaitForSeconds(tickMoveWindow);
            Player2.tickInputActive = false;
            Player1.tickInputActive = false;

            if (Player1.incomingAttack)
            {
                Player1.DamagePlayer();
            }

            if (Player2.incomingAttack)
            {
                Player2.DamagePlayer();
            }
        }

        public void FixedUpdate()
        {
            if (Player1 == null && Player2 == null)
            {
                return;
            }
            var currentPos1 = Player1.currentPosLevel;
            var currentPos2 = Player2.currentPosLevel;

            if (Player1.tickInputActive)
            {
                //Player 1 input types

                if (Input.GetKey(Player1.moveKey_Left))
                {
                    if (currentPos1 != 0)
                    {
                        gameBaseManager.MovePlayerToPos(Player1, currentPos1 - 1);
                    }

                    Player1.tickInputActive = false;
                }

                if (Input.GetKey(Player1.moveKey_Right))
                {
                    if (currentPos1 != 2)
                    {
                        gameBaseManager.MovePlayerToPos(Player1, currentPos1 + 1);
                    }

                    Player1.tickInputActive = false;
                }

                if (Input.GetKey(Player1.attackKey))
                {
                    attackManager.SpawnPlayerAttack(Player1, currentPos1);

                    Player1.tickInputActive = false;
                }

                if (Input.GetKey(Player1.parryKey))
                {
                    if (Player1.incomingAttack)
                    {
                        attackManager.ParryAttack(Player1, Player1.incomingAttack);
                    }
                    Player1.tickInputActive = false;
                }
            }

            if (Player2.tickInputActive)
            {
                //Player 2 input types

                if (Input.GetKey(Player2.moveKey_Left))
                {
                    if (currentPos2 != 2)
                    {
                        gameBaseManager.MovePlayerToPos(Player2, currentPos2 + 1);
                    }

                    Player2.tickInputActive = false;
                }

                if (Input.GetKey(Player2.moveKey_Right))
                {
                    if (currentPos2 != 0)
                    {
                        gameBaseManager.MovePlayerToPos(Player2, currentPos2 - 1);
                    }

                    Player2.tickInputActive = false;
                }

                if (Input.GetKey(Player2.attackKey))
                {
                    attackManager.SpawnPlayerAttack(Player2, currentPos2);

                    Player2.tickInputActive = false;
                }

                if (Input.GetKey(Player2.parryKey))
                {
                    if (Player2.incomingAttack)
                    {
                        attackManager.ParryAttack(Player2, Player2.incomingAttack);
                    }
                    Player2.tickInputActive = false;
                }
            }
        }
    }
}