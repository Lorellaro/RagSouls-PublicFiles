using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Relics;
using MainGame.Utils;
using Player.States;
using Photon.Pun;
using MainGame.UI;

namespace MainGame.Relics
{
    public class Relic : MonoBehaviour
    {
        [SerializeField] RelicData relicDataSO;
        [SerializeField] RelicUIController relicUIController;
        [SerializeField] GameObject pickupVFX;

        protected StateMachine playerStateMachine;
		protected InputManager inputManager;
		protected RelicController relicHandler;
		private bool myPlayerIsInRange;
		private bool isPickedUp;

        public virtual void RelicStart() { }
        public virtual void RelicEnd() { }

        //Called from interact button input
        public void PickupRelic()
        {
			if (isPickedUp)
			{ return; }

			if (myPlayerIsInRange)
			{
				//Pickup relic if in range
				relicHandler.AddRelic(this);
				isPickedUp = true;
                Instantiate(pickupVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
			}
        }


		//When player presses interact button attempt relic pickup
		private void OnEnable()
		{
            relicUIController.relicData = relicDataSO;
            relicUIController.Begin();

			if (inputManager == null)
			{
				inputManager = InputManager.Instance;
			}

			if(relicHandler == null)
			{
				relicHandler = RelicController.Instance;
			}

			inputManager.OnStartInteract += PickupRelic;
		}

		private void OnDisable()
		{
			inputManager.OnStartInteract -= PickupRelic;
		}

		//Player entered sphere
		private void OnTriggerEnter(Collider other)
		{
			if (!other.transform.root.CompareTag("PlayerRoot"))
			{ return; }

			//enable the possibility of picking up as it is now in range
			if (other.transform.root.GetComponent<PhotonView>().IsMine && !myPlayerIsInRange)
			{
				myPlayerIsInRange = true;

                //show pickup item ui
                relicUIController.FadeIn();
			}
		}

		//Player exited sphere
		private void OnTriggerExit(Collider other)
		{
			//disable the possibility of picking up as it is now out of range
			if (other.transform.root.CompareTag("PlayerRoot") && other.transform.root.GetComponent<PhotonView>().IsMine)
			{
				myPlayerIsInRange = false;

                //Minimise Pickup Item UI
                relicUIController.FadeOut();
            }
        }

		public void GetPlayerStateMachine()
        {
            playerStateMachine = AllPlayersHolder.Instance.myStateMachine;
        }
    }
}
