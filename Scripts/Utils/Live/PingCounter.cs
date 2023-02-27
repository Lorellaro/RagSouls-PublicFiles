using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PingCounter : MonoBehaviour
{
	TextMeshProUGUI textComponent;

    void Start()
    {
		textComponent = GetComponent<TextMeshProUGUI>();

		textComponent.SetText("Ping: " + PhotonNetwork.GetPing().ToString());
	}

	void Update()
    {
		textComponent.SetText("Ping: " + PhotonNetwork.GetPing().ToString());
	}
}
