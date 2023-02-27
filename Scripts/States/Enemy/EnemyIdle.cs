using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Utils;
using MainGame.Characters;
using Photon.Pun;

namespace MainGame.Characters.States
{
	public class EnemyIdle : EnemyBaseState
	{

		public override void EnterState(EnemyStateController stateMachine)
		{
			//stateMachine.anim.Play("Idle");
			stateMachine.photonView.RPC("enterAnimation", RpcTarget.All, "Idle");
		}

		public override void UpdateState(EnemyStateController stateMachine)
		{

		}

		public override void FixedUpdateState(EnemyStateController stateMachine)
		{
			//Get closest player
			//Debug.Log(stateMachine.allPlayersHolder.AllPlayersTransforms.ToArray()[0].name);

			Vector3 closestPlayerPos = ClosestTargetFinder.GetClosestTarget(stateMachine.hipRoot.transform, stateMachine.allPlayersHolder.AllPlayersTransforms.ToArray()).position;

			//Get the distance away from player
			//Move toward player if in range
			if (Vector3.Distance(stateMachine.hipRoot.transform.position, closestPlayerPos) < stateMachine.noticeRange)
			{
				//Move toward player if in range
				//stateMachine.hipRoot.velocity += closestPlayerPos * stateMachine.liveStats.getStatByString("Movement Speed") * Time.deltaTime;
				stateMachine.SwitchState(stateMachine.runState);
			}

			//Get all players
			//get closest one to me
			//follow it
			//if in range
			//attack it
		}

	}
}
