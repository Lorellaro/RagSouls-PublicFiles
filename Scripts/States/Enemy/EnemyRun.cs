using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.States;
using MainGame.Characters.Weapons;
using MainGame.Utils;
using Photon.Pun;

namespace MainGame.Characters.States
{
	public class EnemyRun : EnemyBaseState
	{
		Vector3 closestPlayerPos;

		public override void EnterState(EnemyStateController stateMachine)
		{
			//stateMachine.anim.Play("Run");
			stateMachine.photonView.RPC("enterAnimation", RpcTarget.All, "Run");
		}

		public override void UpdateState(EnemyStateController stateMachine)
		{
			closestPlayerPos = ClosestTargetFinder.GetClosestTarget(stateMachine.hipRoot.transform, stateMachine.allPlayersHolder.AllPlayersTransforms.ToArray()).position;

			if (Vector3.Distance(stateMachine.hipRoot.transform.position, closestPlayerPos) < stateMachine.attackRange)
			{
				stateMachine.SwitchState(stateMachine.attackState);
			}
		}

		public override void FixedUpdateState(EnemyStateController stateMachine)
		{
			//Vector3 closestPlayerPos = ClosestTargetFinder.GetClosestEnemy(stateMachine.hipRoot.transform, stateMachine.allPlayersHolder.AllPlayersTransforms.ToArray()).position;

			Vector3 playerDirection = (closestPlayerPos - stateMachine.hipRoot.transform.position).normalized;

			playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z);


			float targetAngle = Mathf.Atan2(playerDirection.z, playerDirection.x) * Mathf.Rad2Deg;

			stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 90, 0f);

			stateMachine.hipRoot.velocity +=  playerDirection * stateMachine.liveStats.getStatByString("Movement Speed") * Time.deltaTime;
		}

        public override void AttemptRagdoll(EnemyStateController stateMachine)
        {
            stateMachine.SwitchStateRagdoll();
        }

    }
}
