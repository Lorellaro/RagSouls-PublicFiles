using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.States;

namespace Player.States
{
	public class StrafeState : BaseState
	{
		public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }
			myAnim = stateMachine.targetAnimator;
			myAnim.Play("StrafeRight");
		}

		public override IEnumerator Start(StateMachine _stateMachine)
		{
			yield break;
		}

		public override void UpdateState(StateMachine stateMachine)
		{
			if (stateMachine.view.IsMine)
			{
				//Check if idle
				if (stateMachine.Horizontal == 0 && stateMachine.Vertical == 0)
				{
					stateMachine.SwitchState(stateMachine.idleState);
				}

				//fall
				if ((stateMachine.shinLScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround && stateMachine.shinRScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround))
				{
					stateMachine.SwitchState(stateMachine.fallState);
				}
			}
		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
			if (stateMachine.view.IsMine)
			{
				Vector3 direction = new Vector3(-stateMachine.transform.position.x, 0f, -stateMachine.transform.position.y).normalized;
				direction = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * direction;

				if (direction.magnitude >= 0.1f)//Manipulate rootHip to face target
				{
					float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

					stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 220, 0f);
				}

				Vector3 StrafeDir = new Vector3(stateMachine.Horizontal, 0f, stateMachine.Vertical).normalized;
				StrafeDir = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * StrafeDir;

				if (StrafeDir.magnitude >= 0.1f)
				{
					stateMachine.hip.velocity = (StrafeDir * stateMachine.liveStats.getStatByString("Movement Speed") * Time.deltaTime
						+ stateMachine.hip.transform.up * stateMachine.runUpForce * Time.deltaTime);
				}

				if (stateMachine.lockonScript.enabled)//is locked on
				{
					Vector3 forwardDir = new Vector3(stateMachine.mainCamera.forward.x, 0f, stateMachine.mainCamera.forward.z).normalized;
					Vector3 newdirection = new Vector3(-stateMachine.transform.position.x, 0f, -stateMachine.transform.position.y).normalized;
					newdirection = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * newdirection;

					if (newdirection.magnitude >= 0.1f)//Manipulate rootHip to face target
					{
						float targetAngle = Mathf.Atan2(forwardDir.z, forwardDir.x) * Mathf.Rad2Deg;

						stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 180, 0f);
					}
				}
			}
		}

		public override void AttemptJump(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.jumpState);
		}

		public override void AttemptAttack(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.attackState1);
		}

		//Strong Attack Transition
		public override void AttemptStrongAttack(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.strongAttackState1);
		}

		//Dodge Transition
		public override void AttemptDodge(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.diveState);
		}

		public override void AttemptRagdoll(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.fullRagdollState);
		}
	}
}
