using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.States;

namespace Player.States
{
	public class FallState : BaseState
	{
		float timeSinceStart = 0;

		public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			timeSinceStart = 0.15f;

			myAnim = stateMachine.targetAnimator;
			myAnim.Play("Fall");
		}

		public override void UpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

            timeSinceStart -= Time.deltaTime;

			if(timeSinceStart > 0)
			{ return; }

			//Transition state
			if (stateMachine.shinLScript.getTimeSinceLeftFloor() < stateMachine.timeSinceTouchGround || stateMachine.shinRScript.getTimeSinceLeftFloor() < stateMachine.timeSinceTouchGround)
			{
				if(stateMachine.Horizontal == 0 && stateMachine.Vertical == 0) //Idle
				{
					stateMachine.SwitchState(stateMachine.idleState);
				}
				else //Run
				{
					stateMachine.SwitchState(stateMachine.runState);
				}
			}

			//if jump run fall attack
			//Ragdoll
			if (Input.GetKeyDown(KeyCode.P))
			{
				stateMachine.SwitchState(stateMachine.fullRagdollState);
			}
		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			//fall movementControl

			Vector3 direction = new Vector3(stateMachine.Horizontal, 0f, stateMachine.Vertical).normalized;
			direction = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * direction;

			if (direction.magnitude >= 0.1f)
			{
				float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

				stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 180, 0f);

				//this.hip.AddForce(direction * this.speed * Time.deltaTime);
				stateMachine.hip.velocity += (direction * stateMachine.fallControlSpeed * Time.deltaTime);
			}


			//fall multi
			//stateMachine.hip.velocity += Vector3.up * Physics.gravity.y * (stateMachine.fallMulti - 1) * Time.deltaTime;
			stateMachine.hip.AddForce(Vector3.up * Physics.gravity.y * (stateMachine.fallMulti - 1) * Time.deltaTime);
		}

        //Descend Attack Transition
        public override void AttemptAttack(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            stateMachine.SwitchState(stateMachine.descendAttackState);
            /*
            if (damageEnabledTime <= 0)
            {
                //Queue next attack once this ones
                nextAttackQueued = true;
            }
            */
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
