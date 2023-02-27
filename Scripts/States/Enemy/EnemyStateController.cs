using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Player.States;
using MainGame.Characters.States;
using MainGame.Characters.Ragdoll;
using MainGame.Characters.BaseStats;
using MainGame.Utils;
using MainGame.Characters.AttackData;
using MainGame.Characters.Weapons;

namespace MainGame.Characters.States
{
	public class EnemyStateController : MonoBehaviour
	{
		[Header("Attack")]
		public float attackRange;
		public List<AttackDataSO> attackDataScriptableObj;
        public GameObject GrowlSFX;
		//public List<AnimationClip> attackAnimClips;

		[Header("AI")]
		public float noticeRange;

		[Header("Ragdoll")]
		public float ragdollTime = 3f;
        public GameObject FishGruntSFX;
		public List<Rigidbody> allRigidbodies;
		public List<ConfigurableJoint> allPlayerJoints;
		public ConfigurableJoint hipJoint;
		public Rigidbody hipRoot;//Root
        public float ragdolledJointForce;
		public float ragdollFallMultiplier;
		[HideInInspector] public AllPlayersHolder allPlayersHolder;
		[HideInInspector] public ClosestTargetFinder closestTargetFinder;
		[HideInInspector] public List<float> playerJointXsStartingValues = new List<float>();
		[HideInInspector] public List<float> playerJointYZsStartingValues = new List<float>();
        [HideInInspector] public Vector3 fullRagdollKnockbackDirection;
        [HideInInspector] public float hitObjKnockbackForce;
		[HideInInspector] public PhotonView photonView;

        public Animator anim;

		public LiveStats liveStats;

		[Header("UI")]
		public Slider myHealthSlider;
		public MultipleImageFade healthFader;
		EnemyBaseState currentState;

		public EnemyIdle idleState = new EnemyIdle();
		public EnemyRun runState = new EnemyRun();
		public EnemyAttack attackState = new EnemyAttack();
        public EnemyRagdoll ragdollState = new EnemyRagdoll();
		public EnemyDeath deathState = new EnemyDeath();

		float defaultDrag;

		void Start()
		{
			for (int i = 0; i < allPlayerJoints.Count; i++)
			{
				playerJointXsStartingValues.Add(allPlayerJoints[i].angularXDrive.positionSpring);
				playerJointYZsStartingValues.Add(allPlayerJoints[i].angularYZDrive.positionSpring);
			}

			liveStats = GetComponent<LiveStats>();
			allPlayersHolder = AllPlayersHolder.Instance;

			//Setup drag default val
			defaultDrag = hipRoot.drag;

			//setup max health val onslider
			float startingHealthVal = liveStats.getStatByString("Health");
			myHealthSlider.maxValue = startingHealthVal;
			myHealthSlider.value = startingHealthVal;

			photonView = transform.root.GetComponent<PhotonView>();

			currentState = idleState;
			currentState.EnterState(this);
		}

		void Update()
		{
			if (!PhotonNetwork.IsMasterClient)
			{ return; }
			currentState.UpdateState(this);
		}

		private void FixedUpdate()
		{
			if (!PhotonNetwork.IsMasterClient)
			{ return; }
			currentState.FixedUpdateState(this);
		}

		//called from  collider listener acts as a regular oncolenter but for all children
		public void CollisionEnter(Collision collision)
		{
			if(currentState == attackState)
			{
				currentState.OnCollEnter(this, collision);
			}
		}

		public void SwitchState(EnemyBaseState _state)
		{
			currentState = _state;
			currentState.EnterState(this);
		}

		public void SwitchStateRagdoll()
		{
			currentState = ragdollState;
			currentState.EnterRagdollState(this);
		}

		public void resetDrag()
		{
			for(int i = 0; i < allRigidbodies.Count; i++)
			{
				allRigidbodies[i].drag = defaultDrag;
			}
		}

		//Called when any player successfully hits an enemy
        public void enemyHit(float attackDamage, float poiseDisruption, Vector3 knockbackDirection, float knockbackForce)
        {
			if (PhotonNetwork.IsMasterClient)
			{
				fullRagdollKnockbackDirection = knockbackDirection;
				currentState.AttemptRagdoll(this);
				hitObjKnockbackForce = knockbackForce;
			}
			else
			{
				//inform master client to perform attack reciever
				photonView.RPC("otherPlayerEnemyHit", RpcTarget.AllBuffered, poiseDisruption, knockbackDirection, knockbackForce);
			}
			//Make them all into pun rpc calls?


			//take away health
			float healthToTake = attackDamage; 

			liveStats.decrStatByString("Health", healthToTake);

			//update health for everyone
			photonView.RPC("UpdateThisHealthForAll", RpcTarget.AllBuffered, healthToTake);

			/*
			myHealthSlider.value = newHealth;
			healthFader.fadeIn();

			if (liveStats.getStatByString("Health") <= 0)
			{
				SwitchState(deathState);
			}
			*/
		}

		[PunRPC]
		public void enterAnimation(string animName)
		{
			anim.Play(animName);
		}

		//Called when player's who aren't the host hit
		[PunRPC]
		public void otherPlayerEnemyHit(float poiseDisruption, Vector3 knockbackDirection, float knockbackForce)
		{
			fullRagdollKnockbackDirection = knockbackDirection;
			currentState.AttemptRagdoll(this);
			hitObjKnockbackForce = knockbackForce;
		}

		[PunRPC]
		public void UpdateThisHealthForAll(float healthToDecr)
		{
			liveStats.decrStatByString("Health", healthToDecr);

			//update health ui
			float newHealth = liveStats.getStatByString("Health");
			myHealthSlider.value = newHealth;
			healthFader.fadeIn();

			if (newHealth <= 0 && PhotonNetwork.IsMasterClient)
			{
				SwitchState(deathState);
			}
		}
    }
}
