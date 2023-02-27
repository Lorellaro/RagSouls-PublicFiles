using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.AttackData;

namespace Player.States
{
	public class IdleState : BaseState
	{
		public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			myAnim = stateMachine.targetAnimator;
			myAnim.Play("Idle");
		}

		public override void UpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			if (stateMachine.Horizontal != 0 || stateMachine.Vertical != 0)
			{
				stateMachine.SwitchState(stateMachine.runState);
			}

			/*
			//Jump
			if (stateMachine.inputManager.OnStartJump())
			{
				stateMachine.SwitchState(stateMachine.jumpState);
			}
			*/

			//Fall
			if ((stateMachine.shinLScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround && stateMachine.shinRScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround))
			{
				stateMachine.SwitchState(stateMachine.fallState);
			}

			/*
			//Attack
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				stateMachine.SwitchState(stateMachine.attackState1);
			}
			*/

			/*
			//Dodge
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				stateMachine.SwitchState(stateMachine.diveState);
			}
			*/

		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
			if (stateMachine.lockonScript.lockedOn)//is locked on
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

		//Jump Transition
		public override void AttemptJump(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.jumpState);
		}

		//Attack Transition
		public override void AttemptAttack(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.attackState1);
		}

		//Attack Transition
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

		//Ragdoll Transition
		public override void AttemptRagdoll(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			stateMachine.SwitchState(stateMachine.fullRagdollState);
		}
	}
}
