using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.BaseStats;
using MainGame.Utils;
using MainGame.Characters.Ragdoll;

public class EnemyController : MonoBehaviour
{
	[SerializeField] RagdollRootHolder ragdollRootHolder;
	[SerializeField] Animator anim;
	[SerializeField] LiveStats liveStats;
	[SerializeField] float noticeRange;
	[SerializeField] float attackRange;

	ClosestTargetFinder closestTargetFinder;

    void FixedUpdate()
    {
		//Get closest player
		//Vector3 closestPlayerPos = ClosestTargetFinder.GetClosestEnemy(ragdollRootHolder.ragdollRoot, AllPlayersHolder.instance.AllPlayersTransforms.ToArray()).position;

		//Get the distance away from player
		//Move toward player if in range
		/*
		if (Vector3.Distance(ragdollRootHolder.ragdollRoot.position, closestPlayerPos) < noticeRange)
		{
			//Move toward player if in range

		}
		*/
		//Get all players
		//get closest one to me
		//follow it
		//if in range
		//attack it
    }
}
