using System.Collections;
using UnityEngine;
using MainGame.Characters.AttackData;

namespace Player.States
{
	public abstract class BaseState
	{
		protected Animator myAnim;

		public abstract void EnterState(StateMachine stateMachine);
		public abstract void UpdateState(StateMachine stateMachine);
		public abstract void FixedUpdateState(StateMachine stateMachine);

		public virtual void AttemptJump(StateMachine stateMachine) {}
		public virtual void AttemptAttack(StateMachine stateMachine) {}
		public virtual void AttemptStrongAttack(StateMachine stateMachine) {}
		public virtual void AttemptDodge(StateMachine stateMachine) {}
		public virtual void AttemptRagdoll(StateMachine stateMachine) {}

        public virtual void CollisionEnter(StateMachine stateMachine, Collision collision) {}

		public virtual IEnumerator Start(StateMachine _stateMachine)
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
