using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Player.States;

namespace Player.States
{
	public class JumpState : BaseState
	{
		float currentJumpTime = 0;
		float currentJumpForceTime = 0;

        bool queueAttack;

		public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			myAnim = stateMachine.targetAnimator;

			//play anim and start timer
			myAnim.Play("Jump");
            queueAttack = false;
			currentJumpTime = stateMachine.jumpTime;
			currentJumpForceTime = stateMachine.jumpForceTime;
			//VFX
			PhotonNetwork.Instantiate(stateMachine.jumpVFX.name, stateMachine.hip.position, Quaternion.identity);
		}

		public override void UpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			currentJumpTime -= Time.deltaTime;
			currentJumpForceTime -= Time.deltaTime;

			//can't transition for a time
			if(currentJumpTime > 0) { return; }

			//Reset drag
			//stateMachine.waitThenResetDrag();
			//Reset drag
			for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
			{
				stateMachine.allRigidbodies[i].drag = 0;
			}

            //If attack is already pressed then play it once jump is finished
            if (queueAttack)
            {
                stateMachine.SwitchState(stateMachine.descendAttackState);
                return;
            }

			//Swap to falling
			stateMachine.SwitchState(stateMachine.fallState);
		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			Vector3 forwardDirection = new Vector3(stateMachine.Horizontal, 0f, stateMachine.Vertical).normalized;
			forwardDirection = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * forwardDirection;

			//if ((stateMachine.shinLScript.getTimeSinceLeftFloor() < stateMachine.timeSinceTouchGround || stateMachine.shinRScript.getTimeSinceLeftFloor() < stateMachine.timeSinceTouchGround) && currentJumpForceTime > 0)
			if (currentJumpForceTime > 0)
			{
				if(stateMachine.jumpForceTime - currentJumpForceTime > 0.105f && !stateMachine.inputManager.jumpHeld)// forces a minimum jump height
				{ return; }

				float baseJumpForce = stateMachine.liveStats.getStatByString("Jump Height");

				//Jump
				stateMachine.hip.velocity = (Vector3.up * baseJumpForce * Time.deltaTime
										  +  forwardDirection * (baseJumpForce / stateMachine.jumpForwardSpeed) * Time.deltaTime);
				//.velocity += //Vector3.up * jumpForce * Time.deltaTime;//
			}


			//Control jump direction
			if (forwardDirection.magnitude >= 0.1f)
			{
				float targetAngle = Mathf.Atan2(forwardDirection.z, forwardDirection.x) * Mathf.Rad2Deg;

				stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 180, 0f);
			}

			//Apply Enhanced Gravity Manually
			//for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
			//{
			//	stateMachine.allRigidbodies[i].drag = stateMachine.jumpDrag;
			//}

            stateMachine.hip.AddForce(Vector3.up * Physics.gravity.y * (stateMachine.fallMulti - 1) * Time.deltaTime);
        }

        //Queue Descend Attack Transition
        public override void AttemptAttack(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            queueAttack = true;
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
