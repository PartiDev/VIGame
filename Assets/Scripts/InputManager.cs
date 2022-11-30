using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private GamePlayer Player1;
        [SerializeField] private GamePlayer Player2;

        [SerializeField] private PlayerAttackManager attackManager;
        [SerializeField] private Metronome metronome;
        [SerializeField] private float tickMoveWindow = 0.1f;

        private void Awake()
        {
            metronome.OnMetronomeTick += AllowPlayerTick;
        }

        public void AllowPlayerTick()
        {
            StartCoroutine(OpenTickWindow());
        }

        private IEnumerator OpenTickWindow()
        {
            Player2.metronomeTickActive = true;
            Player1.metronomeTickActive = true;
            yield return new WaitForSeconds(tickMoveWindow);
            Player2.metronomeTickActive = false;
            Player1.metronomeTickActive = false;
        }

        public void FixedUpdate()
        {
            if (Player1 == null && Player2 == null)
            {
                return;
            }

            //Player 1 input types
            if (!Player1.movementOnCooldown)
            {/*
                if (Input.GetKey(Player1.moveKey_Right))
                {

                    Player1.tickInputActive = false;
                }

                if (Input.GetKey(Player1.attackKey))
                {
                    attackManager.SpawnPlayerAttack(Player1);

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
                */
            }

            //Player 2 input types

            if (!Player2.movementOnCooldown)
            {

            }
        }
    }
}