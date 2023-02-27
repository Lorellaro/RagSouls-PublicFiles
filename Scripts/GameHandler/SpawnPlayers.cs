using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MainGame.Utils;

public class SpawnPlayers : MonoBehaviour
{
	[SerializeField] AllPlayersHolder allPlayerHolder; 

	public GameObject playerPrefab;

	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	public float minZ;
	public float maxZ;

	private void Awake()
	{
		Vector3 randomPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
		PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
		allPlayerHolder.photonView.RPC("RefreshPlayerList", RpcTarget.All);
	}
}
