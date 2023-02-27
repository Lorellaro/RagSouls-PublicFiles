using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.States;
using MainGame.Characters.AttackData;

namespace Player.States
{
	public class RunState : BaseState
	{
		public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine) { return; }
			myAnim = stateMachine.targetAnimator;
			myAnim.Play("Run");
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
				if(stateMachine.Horizontal == 0 && stateMachine.Vertical == 0)
				{
					stateMachine.SwitchState(stateMachine.idleState);
				}

				//check if jumped
				/*
				if (Input.GetKeyDown(KeyCode.Space))
				{
					stateMachine.SwitchState(stateMachine.jumpState);
				}
				*/

				if (stateMachine.lockonScript.lockedOn)
				{
					stateMachine.SwitchState(stateMachine.strafeState);
				}

				//fall
				if ((stateMachine.shinLScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround && stateMachine.shinRScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround))
				{
					stateMachine.SwitchState(stateMachine.fallState);
				}

				//Ragdoll
				if (Input.GetKeyDown(KeyCode.P))
				{
					stateMachine.SwitchState(stateMachine.fullRagdollState);
				}
			}
		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
			if (stateMachine.view.IsMine)
			{


				Vector3 direction = new Vector3(stateMachine.Horizontal, 0f, stateMachine.Vertical).normalized;
				direction = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * direction;

				if (direction.magnitude >= 0.1f)
				{
					float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

					stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 180, 0f);

					//this.hip.AddForce(direction * this.speed * Time.deltaTime);
					stateMachine.hip.velocity = (direction * stateMachine.liveStats.getStatByString("Movement Speed") * Time.deltaTime
						+ stateMachine.hip.transform.up * stateMachine.runUpForce * Time.deltaTime);
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
