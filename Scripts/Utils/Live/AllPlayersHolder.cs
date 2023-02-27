using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MainGame.Characters.Ragdoll;
using Player.States;

namespace MainGame.Utils
{
	public class AllPlayersHolder : Singleton<AllPlayersHolder>
	{
		[SerializeField] bool offlineMode;

		public List<GameObject> AllPlayersObjs;
		public List<Transform> AllPlayersTransforms;
		public AllPlayersHolder instance;
		public GameObject myPlayerRoot;
        public StateMachine myStateMachine;
		public PhotonView photonView;

		void Start()
		{
			if (!offlineMode)
			{
				photonView = GetComponent<PhotonView>();
				photonView.RPC("RefreshPlayerList", RpcTarget.All);
			}
			else
			{
				RefreshPlayerListOFFLINE();
			}
		}

		//Updated every time a new player enters world
		[PunRPC]
		private void RefreshPlayerList()
		{
			AllPlayersObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerRoot"));

			AllPlayersTransforms = new List<Transform>();
			for(int i = 0; i < AllPlayersObjs.Count; i++)
			{
				AllPlayersTransforms.Add(AllPlayersObjs[i].GetComponent<RagdollRootHolder>().ragdollRoot);
			}

			UpdateMyPlayerRoot();
		}

		//Same as default just   for offline use
		private void RefreshPlayerListOFFLINE()
		{
			AllPlayersObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag("PlayerRoot"));

			AllPlayersTransforms = new List<Transform>();
			for (int i = 0; i < AllPlayersObjs.Count; i++)
			{
				AllPlayersTransforms.Add(AllPlayersObjs[i].GetComponent<RagdollRootHolder>().ragdollRoot);
			}

			UpdateMyPlayerRoot();
		}

		//Gets my player's root
		public void UpdateMyPlayerRoot()
		{
			for(int i = 0; i < AllPlayersObjs.Count; i++)
			{
				if (AllPlayersObjs[i].GetComponent<PhotonView>().IsMine)
				{
					myPlayerRoot = AllPlayersObjs[i];
					myStateMachine = myPlayerRoot.GetComponentInChildren<StateMachine>();
				}
			}
		}
	}
}
