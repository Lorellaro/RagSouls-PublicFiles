using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.States;
using MainGame.Utils;
using MainGame.Characters.Weapons;
using Photon.Pun;

namespace MainGame.Characters.States
{
	public class EnemyRagdoll : EnemyBaseState
	{
        float currentRagdollTime;
        float currentForceRagdollTime;

        public override void EnterState(EnemyStateController stateMachine)
		{
			Debug.Log("Wrong Entrance to enemy Ragdoll State");
        }

		public override void EnterRagdollState(EnemyStateController stateMachine)
		{
            //Play SFX
            PhotonNetwork.Instantiate(stateMachine.FishGruntSFX.name, stateMachine.hipRoot.transform.position, Quaternion.identity);
			currentRagdollTime = stateMachine.ragdollTime;
			currentForceRagdollTime = 0.15f;//stateMachine.attackDataSO.playerKnockbackForceDuration;

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
            currentRagdollTime -= Time.deltaTime;
            currentForceRagdollTime -= Time.deltaTime;

            if (currentRagdollTime > 0)
            { return; }

            unRagdollSelf(stateMachine);

            //---Transitions

            Vector3 closestPlayerPos = ClosestTargetFinder.GetClosestTarget(stateMachine.hipRoot.transform, stateMachine.allPlayersHolder.AllPlayersTransforms.ToArray()).position;

            //Get the distance away from player
            //Move toward player if in range
            if (Vector3.Distance(stateMachine.hipRoot.transform.position, closestPlayerPos) < stateMachine.noticeRange)
            {
                //Move toward player if in range
                //stateMachine.hipRoot.velocity += closestPlayerPos * stateMachine.liveStats.getStatByString("Movement Speed") * Time.deltaTime;
                stateMachine.SwitchState(stateMachine.runState);
            }
        }

        public override void FixedUpdateState(EnemyStateController stateMachine)
		{
			if (currentForceRagdollTime > 0)// time force is applied for
			{
				//Debug.Log("Apply force " + stateMachine.fullRagdollKnockbackDirection);//stateMachine.attackDataSO.playerKnockbackForce);
																					   //apply force depending on attack that the player has been hit by
																					   //stateMachine.fullRagdollKnockbackDirection = new Vector3(stateMachine.fullRagdollKnockbackDirection.x, stateMachine.hip.transform.position.y, stateMachine.fullRagdollKnockbackDirection.z);
				stateMachine.hipRoot.velocity = stateMachine.fullRagdollKnockbackDirection * stateMachine.hitObjKnockbackForce * Time.deltaTime;
			}

			stateMachine.hipRoot.velocity += Vector3.down * stateMachine.ragdollFallMultiplier * Time.deltaTime;
		}

        private void unRagdollSelf(EnemyStateController stateMachine)
        {
            //remove force from configurable joints
            for (int i = 0; i < stateMachine.allPlayerJoints.Count; i++)
            {
                //Set X Drive
                JointDrive jointXDrive = stateMachine.allPlayerJoints[i].angularXDrive;
                jointXDrive.positionSpring = stateMachine.playerJointXsStartingValues[i];
                stateMachine.allPlayerJoints[i].angularXDrive = jointXDrive;

                //Set YZ Drive
                JointDrive jointYZDrive = stateMachine.allPlayerJoints[i].angularYZDrive;
                jointYZDrive.positionSpring = stateMachine.playerJointYZsStartingValues[i];
                stateMachine.allPlayerJoints[i].angularYZDrive = jointYZDrive;
            }
        }
    }
}
