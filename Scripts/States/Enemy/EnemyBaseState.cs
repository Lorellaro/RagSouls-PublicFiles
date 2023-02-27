using System.Collections;
using UnityEngine;
using MainGame.Characters.States;
using MainGame.Characters.Weapons;

namespace MainGame.Characters.States
{
	public abstract class EnemyBaseState
	{
		protected Animator myAnim;

		public abstract void EnterState(EnemyStateController stateMachine);
		public abstract void UpdateState(EnemyStateController stateMachine);
		public abstract void FixedUpdateState(EnemyStateController stateMachine);

		public virtual void EnterRagdollState(EnemyStateController stateMachine) {}

		public virtual void OnCollEnter(EnemyStateController stateMachine, Collision collision) {}
        public virtual void AttemptRagdoll(EnemyStateController stateMachine) {}

        public virtual IEnumerator Start(EnemyStateController _stateMachine)
		{
			yield break;
		}

		public virtual IEnumerator Idle()
		{
			yield break;
		}

		public virtual IEnumerator Run()
		{
			yield break;
		}

		public virtual IEnumerator Jump()
		{
			yield break;
		}

		public virtual IEnumerator Fall()
		{
			yield break;
		}
	}
}
