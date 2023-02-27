using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.States;

namespace Player.States
{
	public class StrongAttackState1 : BaseState
	{
		float attackComboTime;
		float attackForceTime;
        float damageEnabledTime;

        bool hasDamageBeenEnabled;

        public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			myAnim = stateMachine.targetAnimator;
			myAnim.Play("Attack 2");
			//stateMachine.currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
			//stateMachine.weaponAnim.enabled = true;
			attackComboTime = 2;
			attackForceTime = stateMachine.attackForceDuration;
            damageEnabledTime = 0.8f;
            hasDamageBeenEnabled = false;

            //Change drag
            stateMachine.hip.drag = stateMachine.newAttackDrag;
			stateMachine.hip.angularDrag = stateMachine.newAttackDrag;
		}

		public override void UpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			attackComboTime -= Time.deltaTime;
			attackForceTime -= Time.deltaTime;

            damageEnabledTime -= Time.deltaTime;

            if (damageEnabledTime <= 0 && !hasDamageBeenEnabled)//amount of time has passed to allow damage dealing
            {
                stateMachine.currentWeapon.EnableDamageDealing(); // turn on damage dealing colliders
                hasDamageBeenEnabled = true;//prevents this if statement from running multiple times
            }

            if (attackForceTime > 0)
			{ return; }



			//Reset drag
			//stateMachine.hip.drag = stateMachine.defaultDrag;
			//stateMachine.hip.angularDrag = stateMachine.defaultDrag;

			if (attackComboTime > 0)
			{ return; }

			stateMachine.currentWeapon.DisableDamageDealing();// turn off damage dealing colliders

			stateMachine.currentWeapon.transform.GetComponent<Rigidbody>().isKinematic = false;
			stateMachine.weaponAnim.enabled = false;

			//Reset drag
			for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
			{
				stateMachine.allRigidbodies[i].drag = 0;
			}

			//Idle Transition
			if (stateMachine.Horizontal == 0 && stateMachine.Vertical == 0)
			{
				stateMachine.SwitchState(stateMachine.idleState);
			}

			//Run Transition
			if (stateMachine.Horizontal != 0 || stateMachine.Vertical != 0)
			{
				stateMachine.SwitchState(stateMachine.runState);
			}

			/*
			//Jump Transition
			if (Input.GetKeyDown(KeyCode.Space))
			{
				stateMachine.SwitchState(stateMachine.jumpState);
			}
			*/

			//fall
			if ((stateMachine.shinLScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround && stateMachine.shinRScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround))
			{
				stateMachine.SwitchState(stateMachine.fallState);
			}
		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
            Vector3 forwardDir = new Vector3(stateMachine.mainCamera.forward.x, 0f, stateMachine.mainCamera.forward.z).normalized;
            Vector3 direction = new Vector3(-stateMachine.transform.position.x, 0f, -stateMachine.transform.position.y).normalized;
            direction = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * direction;

            if (direction.magnitude >= 0.1f)//Manipulate rootHip to face target
            {
                float targetAngle = Mathf.Atan2(forwardDir.z, forwardDir.x) * Mathf.Rad2Deg;

                stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 180, 0f);
            }

            //Apply Enhanced Gravity Manually
            for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
            {
                stateMachine.allRigidbodies[i].drag = stateMachine.newAttackDrag;
            }
            //stateMachine.hipJoint.targetRotation = Quaternion.LookRotation(stateMachine.mainCamera.position - stateMachine.transform.position);//Quaternion.Euler(0f, targetAngle - 180, 0f);
		}

        //Deactivates all changes made since entrance of state
        private static void DisablePlayerStateChanges(StateMachine stateMachine)
        {
            stateMachine.currentWeapon.DisableDamageDealing();// turn off damage dealing colliders

            stateMachine.currentWeapon.transform.GetComponent<Rigidbody>().isKinematic = false;
            stateMachine.weaponAnim.enabled = false;

            //Reset drag
            for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
            {
                stateMachine.allRigidbodies[i].drag = 0;
            }

            stateMachine.ResetJointForcesToInitial();
        }

        //Jump Transition
        public override void AttemptJump(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			if (attackForceTime > 0)
			{ return; }

			if (attackComboTime > 0)
			{ return; }

			stateMachine.SwitchState(stateMachine.jumpState);
		}

		//DodgeTransition
		public override void AttemptDodge(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			if (attackForceTime > 0)
			{ return; }

			if (attackComboTime > 0)
			{ return; }

			stateMachine.SwitchState(stateMachine.diveState);
		}

        //Ragdoll Transition
        public override void AttemptRagdoll(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            DisablePlayerStateChanges(stateMachine);

            stateMachine.SwitchState(stateMachine.fullRagdollState);
        }
    }
}
