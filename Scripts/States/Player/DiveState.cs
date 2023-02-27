using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Player.States;
using Photon.Pun;

namespace Player.States
{
	public class DiveState : BaseState
	{
		float currentDiveTime;
		float currentForceTime;
		float preJumpTime;

		public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			//VFX
			myAnim = stateMachine.targetAnimator;
			myAnim.Play("Dive");
			PhotonNetwork.Instantiate(stateMachine.DodgeVFX.name, stateMachine.hip.transform.position, stateMachine.hip.transform.rotation);

			//Camera shake
			stateMachine.cameraShakeHandler.BasicShake();

            //Callback Function
            stateMachine.OnPlayerDodgeEnter(stateMachine.hip.transform.position);

			//Setup timers
			currentDiveTime = stateMachine.dodgeTime;
			currentForceTime = stateMachine.dodgeForceApplicationTime;
			preJumpTime = stateMachine.timeBeforeDodge;

		}

		public override void UpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			currentDiveTime -= Time.deltaTime;
			preJumpTime -= Time.deltaTime;

			if (preJumpTime <= 0)
			{
				currentForceTime -= Time.deltaTime;
			}

			if (currentDiveTime > 0)
			{ return; }

			//Enable gravity
			for(int i = 0; i < stateMachine.allRigidbodies.Count; i++)
			{
				stateMachine.allRigidbodies[i].useGravity = true;
				stateMachine.allRigidbodies[i].drag = 0;
			}

			//Run
			if (stateMachine.Horizontal != 0 || stateMachine.Vertical != 0)
			{
				stateMachine.SwitchState(stateMachine.runState);
			}

			//Idle
			if(stateMachine.Horizontal == 0 && stateMachine.Vertical == 0)
			{
				stateMachine.SwitchState(stateMachine.idleState);
			}

			/*
			//Jump
			if (Input.GetKeyDown(KeyCode.Space))
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
		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			//Sharp down force after dash
			if (currentDiveTime <= 0.15f)
			{
				stateMachine.hip.velocity += Vector3.down * stateMachine.dodgeEndDownForce * Time.deltaTime;
			}

			if (currentForceTime <= 0 || preJumpTime > 0)
			{ return; }

			Vector3 direction = new Vector3(stateMachine.Horizontal, 0f, stateMachine.Vertical);
			direction = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * direction;

			if (direction.magnitude >= 0.1f)
			{
				float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

				stateMachine.hipJoint.targetRotation = Quaternion.Euler(stateMachine.hipJoint.transform.rotation.x, targetAngle - 180, stateMachine.hipJoint.transform.position.z);

				//this.hip.AddForce(direction * this.speed * Time.deltaTime);
				stateMachine.hip.velocity = (direction.normalized * stateMachine.dodgeForce * Time.deltaTime);
				stateMachine.hip.velocity += Vector3.up * stateMachine.dodgeUpForce * Time.deltaTime;
					
			}

			//Apply Enhanced Gravity Manually
			for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
			{
				stateMachine.allRigidbodies[i].velocity += (Vector3.down * stateMachine.dodgeDownForce * Time.deltaTime);
				stateMachine.allRigidbodies[i].drag = 8f;
			}
		}

		public override void AttemptAttack(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			if(currentDiveTime > 0)
			{ return; }

			stateMachine.SwitchState(stateMachine.attackState1);
		}

		public override void AttemptJump(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			if (currentDiveTime > 0)
			{ return; }

			stateMachine.SwitchState(stateMachine.jumpState);
		}

        //Ragdoll Transition
        public override void AttemptRagdoll(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            //ReEnable gravity
            for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
            {
                stateMachine.allRigidbodies[i].useGravity = true;
                stateMachine.allRigidbodies[i].drag = 0;
            }

            stateMachine.SwitchState(stateMachine.fullRagdollState);
        }
    }
}
