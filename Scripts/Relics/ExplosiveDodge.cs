using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Relics;
using Photon.Pun;

namespace MainGame.Relics
{
    public class ExplosiveDodge : Relic
    {
        [SerializeField] float dodgeForce;
        [SerializeField] GameObject dodgeExplosionVFX;

        public override void RelicStart()
        {
            GetPlayerStateMachine();

            //Get a callback function when entering statemachine
            playerStateMachine.playerDodgeEnter += explodeVFX;
        }

        public override void RelicEnd()
        {
            playerStateMachine.playerDodgeEnter -= explodeVFX;
        }

        private void explodeVFX(Vector3 dodgePos)
        {
            PhotonNetwork.Instantiate(dodgeExplosionVFX.name, dodgePos, Quaternion.identity);
        }
    }
}
