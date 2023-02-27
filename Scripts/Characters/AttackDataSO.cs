using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Characters.AttackData
{
	[CreateAssetMenu(fileName = "Attack Data", menuName = "Enemy Attack Info")]
	public class AttackDataSO : ScriptableObject
	{
		public AnimationClip attackAnimClip;
		public float attackDamage;
		public float attackForce;
		public float attackPoiseDisruption;
		public float hitObjKnockbackForce;
        public float stopTrackingPlayerTime;
		public Vector2 attackForceStartEndTime;
		public Vector2 damageEnabledStartEndTime;
		public float dragAfterAttack;
		public Vector2 dragEffectStartEndTime;
	}
}
