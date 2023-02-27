using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class MainCameraController : MonoBehaviour
{
	CinemachineVirtualCamera vcam;
	PhotonView view;

	// Start is called before the first frame update
	void Start()
    {
		view = GetComponent<PhotonView>();
		if (view.IsMine)
		{
			//vcam.LookAt();
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
