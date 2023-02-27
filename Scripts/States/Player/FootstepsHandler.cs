using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MainGame.Characters
{
    public class FootstepsHandler : MonoBehaviour
    {
        [SerializeField] GameObject footStepSFX;
        [SerializeField] Transform playerLFoot;
        [SerializeField] Transform playerRFoot;
        [SerializeField] bool playViaPhotonNetwork = true;

        public void playFootstepL()
        {
            if (playViaPhotonNetwork)
            {
                PhotonNetwork.Instantiate(footStepSFX.name, playerRFoot.position, Quaternion.identity);
            }
            else
            {
                Instantiate(footStepSFX, playerRFoot.position, Quaternion.identity);
            }
        }

        public void playFootstepR()
        {
            if (playViaPhotonNetwork)
            {
                PhotonNetwork.Instantiate(footStepSFX.name, playerLFoot.position, Quaternion.identity);
            }
            else
            {
                Instantiate(footStepSFX, playerLFoot.position, Quaternion.identity);
            }
        }
    }
}