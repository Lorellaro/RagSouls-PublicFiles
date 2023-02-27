using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.States;
using MainGame.Characters.Weapons;
using MainGame.Cameras;

namespace MainGame.Characters.States
{
	public class EnemyDeath : EnemyBaseState
	{
		float currentForceRagdollTime;

		public override void EnterState(EnemyStateController stateMachine)
		{
			currentForceRagdollTime = 0.15f;
            //Reset Lockon
            //GameObject.FindGameObjectWithTag("vCam").GetComponent<PlayerCamLockOn>().AttemptLockOn();

            stateMachine.anim.Play("Empty");

			//Ragdoll
			//remove force from configurable joints
			for (int i = 0; i < stateMachine.allPlayerJoints.Count; i++)
			{
				//Set X Drive
				JointDrive jointXDrive = stateMachine.allPlayerJoints[i].angularXDrive;
				jointXDrive.positionSpring = stateMachine.ragdolledJointForce;
				stateMachine.allPlayerJoints[i].angularXDrive = jointXDrive;

				//Set YZ Drive
				JointDrive jointYZDrive = stateMachine.allPlayerJoints[i].angularYZDrive;
				jointYZDrive.positionSpring = stateMachine.ragdolledJointForce;
				stateMachine.allPlayerJoints[i].angularYZDrive = jointYZDrive;
			}
		}

		public override void UpdateState(EnemyStateController stateMachine)
		{

		}

		public override void FixedUpdateState(EnemyStateController stateMachine)
		{
			currentForceRagdollTime -= Time.deltaTime;

			if (currentForceRagdollTime > 0)// time force is applied for
			{
				stateMachine.hipRoot.velocity = stateMachine.fullRagdollKnockbackDirection * stateMachine.hitObjKnockbackForce * Time.deltaTime;
			}

			stateMachine.hipRoot.velocity += Vector3.down * stateMachine.ragdollFallMultiplier * Time.deltaTime;
		}

		public override void AttemptRagdoll(EnemyStateController stateMachine)
		{
			stateMachine.resetDrag();
			stateMachine.SwitchStateRagdoll();
		}
	}
}
