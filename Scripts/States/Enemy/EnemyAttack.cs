using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.States;
using Player.States;
using MainGame.Characters.BaseStats;
using MainGame.Characters.Weapons;
using MainGame.Utils;
using Photon.Pun;

namespace MainGame.Characters.States
{
	public class EnemyAttack : EnemyBaseState
	{
		int randNum;

		float attackDuration;
        float playerTrackingTime;
		float currentForceApplicationTime;
		float currentDragApplicationTime;
		float currentDamageApplicationTime;

        bool hasHit = false;
        bool hasAlreadyGrowled;

		RuntimeAnimatorController animController;
		AnimationClip currentClip;
		Vector3 closestPlayerPos;

        Vector3 playerDirection;

		Vector2 forceStartStopTime;
		Vector2 dragStartStopTime;
		Vector2 damageStartStopTime;

		public override void EnterState(EnemyStateController stateMachine)
		{
			//currentForceRagdollTime = stateMachine.attackDataSO.hitObjKnockbackForceDuration;

			//Pick random attack of the three
			randNum = Random.Range(0, stateMachine.attackDataScriptableObj.Count);
            hasHit = false;
            hasAlreadyGrowled = false;

			//Get attack data from scriptable Obj
			string attackClipName = stateMachine.attackDataScriptableObj[randNum].attackAnimClip.name;
			//stateMachine.anim.Play(attackClipName);
			stateMachine.photonView.RPC("enterAnimation", RpcTarget.All, attackClipName);

            //Setup Timers
			attackDuration = stateMachine.attackDataScriptableObj[randNum].attackAnimClip.length;
			forceStartStopTime = stateMachine.attackDataScriptableObj[randNum].attackForceStartEndTime;
			damageStartStopTime = stateMachine.attackDataScriptableObj[randNum].damageEnabledStartEndTime;
			dragStartStopTime = stateMachine.attackDataScriptableObj[randNum].dragEffectStartEndTime;
			playerTrackingTime = stateMachine.attackDataScriptableObj[randNum].stopTrackingPlayerTime;

			currentForceApplicationTime = 0;
			currentDragApplicationTime = 0;
			currentDamageApplicationTime = 0;
		}

		public override void UpdateState(EnemyStateController stateMachine)
		{
			if (!PhotonNetwork.IsMasterClient)
			{ return; }

            //Update Timers
			attackDuration -= Time.deltaTime;
			currentForceApplicationTime += Time.deltaTime;
			currentDragApplicationTime += Time.deltaTime;
			currentDamageApplicationTime += Time.deltaTime;
            playerTrackingTime -= Time.deltaTime;

			if(attackDuration <= 0)
			{
				closestPlayerPos = ClosestTargetFinder.GetClosestTarget(stateMachine.hipRoot.transform, stateMachine.allPlayersHolder.AllPlayersTransforms.ToArray()).position;

				//Can transition
				//Move toward player if in range
				if (Vector3.Distance(stateMachine.hipRoot.transform.position, closestPlayerPos) < stateMachine.noticeRange)
				{
					stateMachine.resetDrag();
					stateMachine.SwitchState(stateMachine.runState);
					return;
				}
			}
		}

		public override void FixedUpdateState(EnemyStateController stateMachine)//Only host do logic
		{
			if (!PhotonNetwork.IsMasterClient)
			{ return; }

            //Rotate to face player
            //false when tracking time set in attack data SO is below 0
            if (playerTrackingTime > 0)
            {
                RotateToFacePlayer(stateMachine);

                if (!hasAlreadyGrowled)
                {
                    PhotonNetwork.Instantiate(stateMachine.GrowlSFX.name, stateMachine.hipRoot.transform.position, Quaternion.identity);
                    hasAlreadyGrowled = true;
                }
            }

            //if between certain moments in the animation apply force toward player
            if (currentForceApplicationTime > forceStartStopTime.x && currentForceApplicationTime < forceStartStopTime.y)
			{
				//apply force to nearest player
				stateMachine.hipRoot.velocity += playerDirection.normalized * stateMachine.attackDataScriptableObj[randNum].attackForce * Time.deltaTime;
			}

			//apply drag between times
			if(currentDragApplicationTime > dragStartStopTime.x && currentDragApplicationTime < dragStartStopTime.y)
			{
				for(int i = 0; i < stateMachine.allRigidbodies.Count; i++)
				{
					stateMachine.allRigidbodies[i].drag = stateMachine.attackDataScriptableObj[randNum].dragAfterAttack;
				}
			}
			else if(currentDragApplicationTime >= dragStartStopTime.y)//called once drag is finished
			{
				//set drag to default
				stateMachine.resetDrag();
			}
		}

		public override void OnCollEnter(EnemyStateController stateMachine, Collision collision)
		{
			//if collided with player and is in a damaging state
			if (collision.gameObject.CompareTag("Player") && currentDamageApplicationTime > damageStartStopTime.x && currentDamageApplicationTime < damageStartStopTime.y)
			{
				Transform playerRoot = collision.gameObject.transform.root;
				StateMachine playerStateMachine = playerRoot.GetChild(0).GetComponent<StateMachine>();

				//if player's poise is less than attack's poise breaker and base poise breaker
				if (playerRoot.GetChild(0).GetComponent<LiveStats>().getStatByString("Poise")
					< stateMachine.attackDataScriptableObj[randNum].attackPoiseDisruption
					+ stateMachine.liveStats.getStatByString("Poise Disruption"))
				{
					//Tell player state machine to try damage and knockback
					//Vector3 knocbackDirection = (closestPlayerPos - stateMachine.hipRoot.transform.position).normalized;
					//knocbackDirection = new Vector3(knocbackDirection.x, 0, knocbackDirection.z);

					float knockbackForce = stateMachine.attackDataScriptableObj[randNum].hitObjKnockbackForce;
                    float attackDamage = stateMachine.attackDataScriptableObj[randNum].attackDamage;
                    if (hasHit) { return; }
					playerStateMachine.playerHit(attackDamage, knockbackForce, collision);    //Deal Damage
                    hasHit = true;
                }

			}
		}

        public override void AttemptRagdoll(EnemyStateController stateMachine)
        {
			stateMachine.resetDrag();
			stateMachine.SwitchStateRagdoll();
		}

        //Rotate towards nearest player
        private void RotateToFacePlayer(EnemyStateController stateMachine)
        {
            closestPlayerPos = ClosestTargetFinder.GetClosestTarget(stateMachine.hipRoot.transform, stateMachine.allPlayersHolder.AllPlayersTransforms.ToArray()).position;

            //Rotate towards nearest player
            playerDirection = (closestPlayerPos - stateMachine.hipRoot.transform.position).normalized;
            playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z);
            float targetAngle = Mathf.Atan2(playerDirection.z, playerDirection.x) * Mathf.Rad2Deg;
            stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 90, 0f);
        }
	}
}

