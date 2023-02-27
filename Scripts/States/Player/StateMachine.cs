using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.UI;
using Player.States;
using MainGame.Cameras;
using MainGame.Characters.AttackData;
using MainGame.Characters.BaseStats;
using MainGame.Characters.Weapons;
using MainGame.GameHandling;
using MainGame.Cameras;

namespace Player.States
{
	public class StateMachine : MonoBehaviour
	{
		[Header("--Attack")]
		[SerializeField] public float attackForce;
		[SerializeField] public float attackForceDuration;
		[SerializeField] public float attackComboResetTime;
		[SerializeField] public float newAttackDrag = 13;

        [Header("--Weapon")]
        [SerializeField] public WeaponController currentWeapon;
        [SerializeField] public Animator weaponAnim;

		[Header("--Falling")]
		[SerializeField] public float fallControlSpeed = 5f;
		[SerializeField] public float timeTillFallMulti = 0.5f;
		[SerializeField] public float fallMulti = 2f;
		[SerializeField] public float timeSinceTouchGround = 0.25f;

		[Header("--Run")]
		[SerializeField] public float runUpForce;
		[SerializeField] public PlayerCamLockOn lockonScript;

		[Header("--Jump")]
		[SerializeField] public float jumpForwardSpeed;
		[SerializeField] public float jumpDrag = 6f;
		[SerializeField] public float jumpTime;
		[SerializeField] public float jumpForceTime = 0.15f;
		[SerializeField] public GameObject jumpVFX;

        [Header("--Ragdoll")]
        [SerializeField] private GameObject playerHitVFX;
        [SerializeField] public GameObject playerHitWallFX;
		[SerializeField] public float ragdollTime = 1f;
		[SerializeField] public List<Rigidbody> allRigidbodies;
		[SerializeField] public List<ConfigurableJoint> allPlayerJoints;
		[SerializeField] public ConfigurableJoint hipJoint;
		[SerializeField] public Rigidbody hip;//Root
		[SerializeField] public IsTouchingGround shinLScript;
		[SerializeField] public IsTouchingGround shinRScript;
		[SerializeField] public Animator targetAnimator;
		[SerializeField] public float ragdolledJointForce;
		[SerializeField] public float ragdollFallMultiplier;
		[HideInInspector] public List<float> playerJointXsStartingValues = new List<float>();
		[HideInInspector] public List<float> playerJointYZsStartingValues = new List<float>();

		[Header("--Dodge Dive")]
		[SerializeField] public GameObject DodgeVFX;
		[SerializeField] public float timeBeforeDodge;
		[SerializeField] public float dodgeTime;
		[SerializeField] public float dodgeForce;
		[SerializeField] public float dodgeUpForce;
		[SerializeField] public float dodgeDownForce;
		[SerializeField] public float dodgeEndDownForce;
		[SerializeField] public float dodgeForceApplicationTime;

        [Header("--UI")]
        [SerializeField] Slider healthSlider;

		[Header("--Other")]
		[SerializeField] private Transform vCamTarget;

		[HideInInspector]
		public InputManager inputManager;

        private List<float> initialAngularXDrives = new List<float>();

		private float timeSinceLeftFloor = 0f;

		private bool run = false;
		private bool walk = false;
		private bool jump = false;

		[HideInInspector]
		public PhotonView view;
		[HideInInspector]
		public Transform mainCamera;
		CinemachineFreeLook vCam;
		[HideInInspector]
		public float defaultDrag;
		[HideInInspector] public float Horizontal;
		[HideInInspector] public float Vertical;
		[HideInInspector] public LiveStats liveStats;

		[HideInInspector] public float currentEnemyAttackForce;
		[HideInInspector] public Vector3 fullRagdollKnockbackDirection;
        [HideInInspector] public event Action<Vector3> playerDodgeEnter;
		[HideInInspector] public CameraShakeHandler cameraShakeHandler;

		BaseState currentState;
		public IdleState idleState = new IdleState();
		public RunState runState = new RunState();
		public JumpState jumpState = new JumpState();
		public FallState fallState = new FallState();
        public DescendAttackState descendAttackState = new DescendAttackState();
		public LightAttackState1 attackState1 = new LightAttackState1();
		public LightAttackState2 attackState2 = new LightAttackState2();
		public LightAttackState3 attackState3 = new LightAttackState3();
		public StrongAttackState1 strongAttackState1 = new StrongAttackState1();
		public FullRagdollState fullRagdollState = new FullRagdollState();
		public DiveState diveState = new DiveState();
		public StrafeState strafeState = new StrafeState();

        EnemySpawner enemySpawner;//will erase after testing

		//add new input system methods
		private void Awake()
		{
			if (inputManager == null)
			{
				inputManager = InputManager.Instance;
			}

//            enemySpawner = EnemySpawner.Instance;
			cameraShakeHandler = CameraShakeHandler.Instance;

            for (int i = 0; i < allPlayerJoints.Count; i++)
            {
                initialAngularXDrives.Add(allPlayerJoints[i].angularXDrive.positionSpring);
            }
        }

        private void OnEnable()
		{
			inputManager.OnStartJump += TryJump;
			inputManager.OnStartAttack += TryAttack;
			inputManager.OnStartStrongAttack += TryStrongAttack;
			inputManager.OnStartDodge += TryDodge;
            inputManager.OnStartInteract += TryInteract;
		}

		private void OnDisable()
		{
			inputManager.OnStartJump -= TryJump;
			inputManager.OnStartAttack -= TryAttack;
			inputManager.OnStartStrongAttack -= TryStrongAttack;
			inputManager.OnStartDodge -= TryDodge;
            inputManager.OnStartInteract -= TryInteract;
        }

        // Start is called before the first frame update
        void Start()
		{
			liveStats = GetComponent<LiveStats>();
			view = transform.parent.GetComponent<PhotonView>();
			if (view.IsMine)
			{
				mainCamera = Camera.main.transform;
				//get vcam
				vCam = GameObject.FindGameObjectWithTag("vCam").GetComponent<CinemachineFreeLook>();

				vCam.Follow = vCamTarget;
				vCam.LookAt = vCamTarget;

				lockonScript = vCam.gameObject.GetComponent<PlayerCamLockOn>();

				healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
				/*
								//get vcams
				GameObject[] vcamObjs = GameObject.FindGameObjectsWithTag("vCam");

				for(int i = 0; i < vcamObjs.Length; i++)
				{
					vCams.Add(vcamObjs[i].GetComponent<CinemachineFreeLook>());
				}

				for(int i = 0; i < vCams.Count; i++)
				{
					vCams[i].Follow = vCamTarget;
					vCams[i].LookAt = vCamTarget;
				}
				*/
				//
				//isTouchingGroundScript = hip.GetComponent<IsTouchingGround>();
				currentState = idleState;
				currentState.EnterState(this);

				defaultDrag = hip.drag;

				for (int i = 0; i < allPlayerJoints.Count; i++)
				{
					playerJointXsStartingValues.Add(allPlayerJoints[i].angularXDrive.positionSpring);
					playerJointYZsStartingValues.Add(allPlayerJoints[i].angularYZDrive.positionSpring);
				}

				healthSlider.maxValue = liveStats.getStatByString("Health");
				healthSlider.value = liveStats.getStatByString("Health");
			}

			//hookup live stats to local values

			//StartCoroutine(currentState.Start(this));
		}

		// Update is called once per frame
		void Update()
		{
			if (!view.IsMine)
			{ return; }

			currentState.UpdateState(this);

            healthSlider.value = liveStats.getStatByString("Health");

            Horizontal = inputManager.movementInput.x;
			Vertical = inputManager.movementInput.y;
		}

		void FixedUpdate()
		{
			if (!view.IsMine)
			{ return; }

			currentState.FixedUpdateState(this);
		}

		public void SwitchState(BaseState _state)
		{
			currentState = _state;
			currentState.EnterState(this);
			//StartCoroutine(currentState.Start(this));
		}

		//Jump button is pressed
		public void TryJump()
		{
			if (!view.IsMine)
			{ return; }

			currentState.AttemptJump(this);
		}

		//Attack button is pressed
		public void TryAttack()
		{
			if (!view.IsMine)
			{ return; }

			currentState.AttemptAttack(this);
		}

		//Strong attack is pressed
		public void TryStrongAttack()
		{
			if (!view.IsMine)
			{ return; }

			currentState.AttemptStrongAttack(this);
		}

		//Dodge is pressed
		public void TryDodge()
		{
			if (!view.IsMine)
			{ return; }

			currentState.AttemptDodge(this);
		}

        //Interact is pressed
        public void TryInteract()
        {
            if (!view.IsMine) { return; }

            //Interact

            //Spawn in enemy
            //enemySpawner.SpawnEnemyRandomly();       
        }

        public void playerCollisionEnter(Collision collision)
        {
            currentState.CollisionEnter(this, collision);
        }

        public BaseState GetCurrentState()
        {
            return currentState;
        }

		//Dodge begin Callback function - used in explosion relic
		public void OnPlayerDodgeEnter(Vector3 dodgePos)
		{
			playerDodgeEnter?.Invoke(dodgePos);
		}

		//Called when enemy hits this player
		public void playerHit(float attackDamage, float attackForce, Collision collision)
		{
			if (!view.IsMine)
			{ return; }

			currentEnemyAttackForce = attackForce;
            liveStats.decrStatByString("Health", attackDamage);

            Vector3 collisionPoint = collision.GetContact(0).point;

            //hit VFX
            PhotonNetwork.Instantiate(playerHitVFX.name, collisionPoint, Quaternion.identity);
            cameraShakeHandler.BasicShake();

            //Calculate knocbackDirection
            Vector3 knockbackDirection = (hip.transform.position - collisionPoint).normalized;
            fullRagdollKnockbackDirection = knockbackDirection;

			currentState.AttemptRagdoll(this);
		}

        public void ResetJointForcesToInitial()
        {
            //Reset joint strength
            for (int i = 0; i < allPlayerJoints.Count; i++)
            {
                JointDrive xJointDrive = allPlayerJoints[i].angularXDrive;
                xJointDrive.positionSpring = initialAngularXDrives[i];
                allPlayerJoints[i].angularXDrive = xJointDrive;

                JointDrive yzJointDrive = allPlayerJoints[i].angularYZDrive;
                yzJointDrive.positionSpring = initialAngularXDrives[i];
                allPlayerJoints[i].angularYZDrive = yzJointDrive;
            }
        }
	}
}
