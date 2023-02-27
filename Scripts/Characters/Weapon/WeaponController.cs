using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MainGame.Characters.BaseStats;
using MainGame.Characters.States;
using MainGame.Characters.AttackData;
using Player.States;
using MainGame.Characters.Ragdoll;
using MainGame.Utils;
using MainGame.Cameras;

namespace MainGame.Characters.Weapons
{
	public class WeaponController : MonoBehaviour
	{
		[SerializeField] WeaponInfoSO weaponData;
		[SerializeField] LiveStats playerLiveStats;
        [SerializeField] WeaponStats weaponLiveStats;
        [SerializeField] StateMachine playerStateMachine;
		[SerializeField] GiveRandomAudioClip swingSFX;
		[SerializeField] GameObject collisionSFX;
		[SerializeField] GameObject collisionVFX;
		[SerializeField] GameObject hitEnemyVFX;
		[SerializeField] ParticleSystem swingVFX;
		[SerializeField] bool canCurrentlyDealDamage;
		PhotonView view;

        public int currentAttackNum;
		private CameraShakeHandler cameraShakeHandler;

        public bool hasHitGround;
		bool hasPlayedFXThisSwing = false;

		private List<Transform> enemiesAttacked = new List<Transform>();

		private void Awake()
		{
			view = transform.root.GetComponent<PhotonView>();
		}

		private void Start()
		{
			cameraShakeHandler = CameraShakeHandler.Instance;
		}

		//each animation can only deal damage to the enemy once

		//Called from player statemachine
		public void EnableDamageDealing()
		{
			StartCoroutine(DMGDelay());
		}

		//Called from player statemachine
		public void DisableDamageDealing()
		{
			canCurrentlyDealDamage = false;
            hasHitGround = false;
            //swingVFX.SetActive(false);
            swingVFX.Stop();

		}

		private IEnumerator DMGDelay()
		{
			//Enable VFX and SFX
			//swingVFX.SetActive(true);
			swingVFX.Play();
			swingSFX.changeAndPlayClip();

			yield return new WaitForSeconds(0.01f);//set this to a value to offset the dmg dealing and vfx from the initial swing SFX and VFX

			canCurrentlyDealDamage = true;
			hasPlayedFXThisSwing = false; // Allows vfx and sfx to be played in on coll enter
			enemiesAttacked = new List<Transform>();
		}

		//Resets Vfx on weapon so that it can be played multiple times per swing
		private IEnumerator VFXReEnabler()
		{
			yield return new WaitForSeconds(0.12f);
			hasPlayedFXThisSwing = false; // Allows vfx and sfx to be played in on coll enter
        }

        private void OnCollisionEnter(Collision collision)
		{
			if (!view.IsMine)
			{ return; }

			if (canCurrentlyDealDamage && !hasPlayedFXThisSwing)//if swinnging but not hit an enemy
			{
				//play hit sfx and disable till next swing

				if (collision.transform.CompareTag("Ground"))
				{
					Vector3 spawnPos = collision.GetContact(0).point;
					PhotonNetwork.Instantiate(collisionVFX.name, spawnPos, Quaternion.identity);
					PhotonNetwork.Instantiate(collisionSFX.name, spawnPos, Quaternion.identity);
					hasPlayedFXThisSwing = true;
                    hasHitGround = true;
                    StartCoroutine(VFXReEnabler());
				}
			}

			if (canCurrentlyDealDamage && collision.transform.root.CompareTag("Enemy"))
			{
                //if (collision.gameObject.CompareTag("Player") && currentDamageApplicationTime > damageStartStopTime.x && currentDamageApplicationTime < damageStartStopTime.y)
                //{
                Transform enemyRoot = collision.gameObject.transform.root;

				//exit if already hit this enemy this animation
				if (!canAttackThisEnemy(enemyRoot))
				{ return; }

                EnemyStateController enemyStateMachine = enemyRoot.GetComponent<EnemyStateController>();
                LiveStats enemyLiveStats = enemyRoot.GetComponent<LiveStats>();

                float attackPoise =
                    weaponData.weaponAttackData[currentAttackNum].attackPoiseDisruption // attack's poise disruption
                    + playerLiveStats.getStatByString("Poise Disruption") // player's poise disruption
                    + weaponLiveStats.getStatByString("Poise Disruption"); // weapon's poise disruption

				Vector3 enemyRootPosition = enemyRoot.GetComponent<RagdollRootHolder>().ragdollRoot.position;

				//if enemy's poise is less than attack's poise breaker + base poise breaker + weapon's poise breaker
				if (enemyLiveStats.getStatByString("Poise") < attackPoise)
                {
                    //Tell player state machine to try damage and knockback
                    Vector3 knocbackDirection = (enemyRootPosition - transform.position).normalized;
                    //knocbackDirection = new Vector3(knocbackDirection.x, 0, knocbackDirection.z);

					//KNOCKBACK FORCE CALCULATION
                    float knockbackForce = weaponData.weaponAttackData[currentAttackNum].hitObjKnockbackForce
                                           + weaponLiveStats.getStatByString("Knockback Force")
                                           + playerLiveStats.getStatByString("Attack Knockback");

					//ATTACK DAMAGE CALCULATION
					float attackDamage = weaponData.weaponAttackData[currentAttackNum].attackDamage
										 + weaponLiveStats.getStatByString("Attack")
										 + playerLiveStats.getStatByString("Attack Damage");

                    enemyStateMachine.enemyHit(attackDamage, currentAttackNum, knocbackDirection, knockbackForce);
                }

				//VFX
				PhotonNetwork.Instantiate(hitEnemyVFX.name, enemyRootPosition, Quaternion.identity);

				//Camera Shake
				cameraShakeHandler.BasicShake();

				/*
                //Deal Damage and status effects

                float damageToDeal = 
                    playerLiveStats.getStatByString("Attack Damage") //Player's damage
                    + weaponLiveStats.getStatByString("Attack") // Weapon's damage
                    + weaponData.weaponAttackData[currentAttackNum].attackDamage; // Attack's damage

                enemyLiveStats.decrStatByString("Health", damageToDeal);
				*/

                //Debug.Log("Deal damage");
                //collision.transform.root.GetComponent<EnemyStateController>().enemyHit(attackDataScriptableObjs[currentAttackNum], attackData, );
			}
		}

		//populate list with transforms hit this attack animation
		public bool canAttackThisEnemy(Transform transform)
		{
			for(int i = 0; i < enemiesAttacked.Count; i++)
			{
				if (enemiesAttacked[i] == transform.root)
				{
					return false;
				}
			}

			enemiesAttacked.Add(transform.root);
			return true;
		}
	}
}
