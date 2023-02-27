using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using MainGame.Characters.Ragdoll;
using MainGame.Characters.BaseStats;

namespace MainGame.Cameras
{
	public class PlayerCamLockOn : MonoBehaviour
	{
		[SerializeField] LayerMask targetLayers;
		[SerializeField] float lockonDistance = 10f;
		[SerializeField] float maxNoticeAngle;
		[SerializeField] bool zeroVertLook;
		[SerializeField] GameObject lockOnCanvas;
		[SerializeField] CinemachineVirtualCamera lockOnVCam;

		float currentYOffset;
		public bool lockedOn;

		CinemachineFreeLook freelookCam;
		LockOnUIController lockOnUIController;
		PhotonView _view;
		InputManager _inputManager;
		Transform cam;
		[HideInInspector] public Transform lockOnTarget;

		private void OnEnable()
		{
			if(_view.IsMine)
				_inputManager.OnStartLockOn += AttemptLockOn;
		}

		private void OnDisable()
		{
			if (_view.IsMine)
				_inputManager.OnStartLockOn -= AttemptLockOn;
		}

		private void Awake()
		{
			_inputManager = InputManager.Instance;
			_view = GetComponent<PhotonView>();
			freelookCam = GetComponent<CinemachineFreeLook>();
			lockOnUIController = lockOnCanvas.GetComponent<LockOnUIController>();
			cam = Camera.main.transform;
		}

		public void AttemptLockOn()
		{
			if (!_view.IsMine)
			{ return; }

			if (lockedOn)
			{
				//Swap to unlocked Cam
				disableLockOn();
				return;
			}

			//search area around where camera is  facing and infront
			lockOnTarget = ScanNearBy();

			if(lockOnTarget != null)
			{

				lockOnTarget = lockOnTarget.root.GetComponent<RagdollRootHolder>().ragdollRoot;

				//Swap to Lock on Cam
				freelookCam.m_Priority = 1;
				lockOnVCam.m_Priority = 10;
				lockOnVCam.m_LookAt = lockOnTarget;
				lockOnCanvas.SetActive(true);
				lockOnUIController.SetFollowTarget(lockOnTarget);
				lockedOn = true;
			}

			/*
			RaycastHit hit;
			if (Physics.Raycast(cam.position, cam.forward * 0.5f, out hit, lockonDistance))
			{
				Collider[] nearbyTargets = Physics.OverlapSphere(hit.transform.position, lockonDistance, targetLayers);
				float closestAngle = maxNoticeAngle;
				Transform closestTarget = null;

				if (nearbyTargets.Length <= 0)
					return;

				for (int i = 0; i < nearbyTargets.Length; i++)
				{
					Vector3 dir = nearbyTargets[i].transform.position - cam.position;
					dir.y = 0;
					float _angle = Vector3.Angle(cam.forward, dir);

					if (_angle < closestAngle)
					{
						closestTarget = nearbyTargets[i].transform;
						closestAngle = _angle;
					}
				}

			}
			*/
			//fire out ray then at  either collision or end point of ray search for object within an overlapping sphere
			//then check if object has tag enemy
			//if so set vcam to look  at target to it
		}

		private Transform ScanNearBy()
		{
			Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, lockonDistance, targetLayers);
			float closestAngle = maxNoticeAngle;
			Transform closestTarget = null;

			if (nearbyTargets.Length <= 0)
				return null;

			for(int i = 0; i < nearbyTargets.Length; i++)
			{
				LiveStats targetLiveStats = nearbyTargets[i].transform.root.GetComponent<LiveStats>();
				if (targetLiveStats != null && targetLiveStats.getStatByString("Health") <= 0)
				{ continue; }//check if dead

				Vector3 dir = nearbyTargets[i].transform.position - cam.position;
				dir.y = 0;
				float _angle = Vector3.Angle(cam.forward, dir);

				if(_angle < closestAngle)
				{
					closestTarget = nearbyTargets[i].transform;
					closestAngle = _angle;
				}
			}

			if (!closestTarget)
				return null;

			/*
			float h1 = closestTarget.GetComponent<Collider>().height;
			float h2 = closestTarget.localScale.y;
			float h = h1 * h2;
			float half_h = (h / 2) / 2;
			currentYOffset = h - half_h;
			*/
			//if (zeroVertLook && currentYOffset > 1.6f && currentYOffset < 1.6f * 3) currentYOffset = 1.6f;

			//Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);
			//if (Blocked(closestTarget.position))
			//	return null;

			return closestTarget;
		}

		bool Blocked(Vector3 t)
		{
			RaycastHit hit;
			if (Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out hit))
			{
				if (!hit.transform.CompareTag("Enemy"))
					return true;
			}
			return false;
		}

		private void disableLockOn()
		{
			lockedOn = false;
			freelookCam.m_Priority = 10;
			lockOnVCam.m_Priority = 1;
			lockOnCanvas.SetActive(false);
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, lockonDistance);
		}
	}
}
