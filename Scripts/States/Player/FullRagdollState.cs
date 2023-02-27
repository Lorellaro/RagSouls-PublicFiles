using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.States;
using Photon.Pun;

namespace Player.States
{
	public class FullRagdollState : BaseState
	{
		float currentRagdollTime;
		float currentForceRagdollTime;

        bool canPlayWallHitFX;

		public override void EnterState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			currentRagdollTime = stateMachine.ragdollTime;
            stateMachine.targetAnimator.Play("Empty"); // prevents run anim sfx from playing whilst in ragdolling
			currentForceRagdollTime = 0.15f;

			//Ragdoll
			//remove force from configurable joints
			for(int i = 0; i < stateMachine.allPlayerJoints.Count; i++)
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

            canPlayWallHitFX = true;
        }

		public override void UpdateState(StateMachine stateMachine)
		{
			if (!stateMachine.view.IsMine)
			{ return; }

			currentRagdollTime -= Time.deltaTime;
			currentForceRagdollTime -= Time.deltaTime;

			if(currentRagdollTime > 0)
			{ return; }

			unRagdollSelf(stateMachine);

			//---Transitions

			//Idle
			if (stateMachine.Horizontal == 0 && stateMachine.Vertical == 0)
			{
				stateMachine.SwitchState(stateMachine.idleState);
			}

			//Run
			if (stateMachine.Horizontal != 0 || stateMachine.Vertical != 0)
			{
				stateMachine.SwitchState(stateMachine.runState);
			}
		}

		public override void FixedUpdateState(StateMachine stateMachine)
		{
			if(currentForceRagdollTime > 0)
			{
				Debug.Log("Apply force " + stateMachine.currentEnemyAttackForce);//stateMachine.attackDataSO.playerKnockbackForce);
                                                                                 //apply force depending on attack that the player has been hit by
                                                                                 //stateMachine.fullRagdollKnockbackDirection = new Vector3(stateMachine.fullRagdollKnockbackDirection.x, stateMachine.hip.transform.position.y, stateMachine.fullRagdollKnockbackDirection.z);
                //Take absolute position of the y axis so that players are never hit straight into the floor
                //May cause issues with some enemy attacks
                Vector3 knockbackDir = new Vector3(stateMachine.fullRagdollKnockbackDirection.x, Mathf.Abs(stateMachine.fullRagdollKnockbackDirection.y), stateMachine.fullRagdollKnockbackDirection.z);
                stateMachine.hip.velocity = (knockbackDir * stateMachine.currentEnemyAttackForce * Time.deltaTime);
                                         // +  (Vector3.up * (stateMachine.currentEnemyAttackForce/200f) * Time.deltaTime);
			}

            //stateMachine.hip.velocity += Vector3.down * stateMachine.ragdollFallMultiplier * Time.deltaTime;

            //fall multi
            stateMachine.hip.AddForce(Vector3.up * Physics.gravity.y * (stateMachine.fallMulti/2 - 1) * Time.deltaTime);
        }

        public override void CollisionEnter(StateMachine stateMachine, Collision collision)
        {
            //Get the fastest Velocity of any axis
            float highestPlayerVelocity = 0;
            Vector3 hipVelocity = stateMachine.hip.velocity;

            for(int i = 0; i < 3; i++)
            {
                float absoluteVelocity = Mathf.Abs(hipVelocity[i]);
                if(absoluteVelocity > highestPlayerVelocity)
                {
                    highestPlayerVelocity = absoluteVelocity;
                }
            }

            //ignore if not moving fast
            if(highestPlayerVelocity < 6f)
            {
                return;
            }

            //Disabled after hitting wall for a short period
            if (canPlayWallHitFX && (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall")))
            {
                //Play floor/wall hit fx
                PhotonNetwork.Instantiate(stateMachine.playerHitWallFX.name, collision.contacts[0].point, Quaternion.identity);
                stateMachine.cameraShakeHandler.BasicShake();
                stateMachine.StartCoroutine(timeBtwWallHitFXs());
            }
        }


        private void unRagdollSelf(StateMachine stateMachine)
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

        private IEnumerator timeBtwWallHitFXs()
        {
            canPlayWallHitFX = false;
            yield return new WaitForSeconds(0.4f);
            canPlayWallHitFX = true;
        }
	}
}
