using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MainGame.Utils
{
	public class SetOfflineMode : MonoBehaviour
	{
		[SerializeField] bool startOfflineMode = false;

		void Awake()
		{
			if (startOfflineMode)
			{
				PhotonNetwork.OfflineMode = true;
			}
		}

	}
}
