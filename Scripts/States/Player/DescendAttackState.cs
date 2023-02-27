using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.States
{
    public class DescendAttackState : BaseState
    {
        bool hasDamageBeenEnabled = false;

        public override void EnterState(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            myAnim = stateMachine.targetAnimator;
            myAnim.Play("Descend Attack");
            stateMachine.currentWeapon.EnableDamageDealing(); // turn on damage dealing colliders
            hasDamageBeenEnabled = true;//prevents this if statement from running multiple times
            /*
            //Change drag
            stateMachine.hip.drag = stateMachine.newAttackDrag;
            stateMachine.hip.angularDrag = stateMachine.newAttackDrag;
            */

            //Increase limb strength significantly
            for (int i = 0; i < stateMachine.allPlayerJoints.Count; i++)
            {
                JointDrive xJointDrive = stateMachine.allPlayerJoints[i].angularXDrive;
                xJointDrive.maximumForce = 50000;
                stateMachine.allPlayerJoints[i].angularXDrive = xJointDrive;

                JointDrive yzJointDrive = stateMachine.allPlayerJoints[i].angularYZDrive;
                yzJointDrive.maximumForce = 50000;
                stateMachine.allPlayerJoints[i].angularYZDrive = yzJointDrive;
            }
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            //if touching ground then stop
            if ((stateMachine.shinLScript.getTimeSinceLeftFloor() < stateMachine.timeSinceTouchGround 
             && stateMachine.shinRScript.getTimeSinceLeftFloor() < stateMachine.timeSinceTouchGround)
             || stateMachine.currentWeapon.hasHitGround)// has weapon hit the ground already?
            {
                stateMachine.currentWeapon.DisableDamageDealing();
                hasDamageBeenEnabled = false;
            }

            if (hasDamageBeenEnabled) { return; }

            stateMachine.currentWeapon.DisableDamageDealing();// turn off damage dealing colliders

            stateMachine.currentWeapon.transform.GetComponent<Rigidbody>().isKinematic = false;
            stateMachine.weaponAnim.enabled = false;

            //Reset drag
            for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
            {
                stateMachine.allRigidbodies[i].drag = 0;
            }

            //Reset joint strength
            stateMachine.ResetJointForcesToInitial();

            //Transitions

            /*
            //Next attack in combo Transition
            if (nextAttackQueued)
            {
                stateMachine.SwitchState(stateMachine.attackState2);
                return;
            }
            */

            //Idle Transition
            if (stateMachine.Horizontal == 0 && stateMachine.Vertical == 0)
            {
                stateMachine.SwitchState(stateMachine.idleState);
            }

            //Run Transition
            if (stateMachine.Horizontal != 0 || stateMachine.Vertical != 0)
            {
                stateMachine.SwitchState(stateMachine.runState);
            }

            /*
			//Jump Transition
			if (Input.GetKeyDown(KeyCode.Space))
			{
				stateMachine.SwitchState(stateMachine.jumpState);
			}
			*/

            //fall
            if ((stateMachine.shinLScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround && stateMachine.shinRScript.getTimeSinceLeftFloor() > stateMachine.timeSinceTouchGround))
            {
                stateMachine.SwitchState(stateMachine.fallState);
            }
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {


            //Vector3 direction = new Vector3(stateMachine.Horizontal, 0f, stateMachine.Vertical).normalized;
            //direction = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * direction;

            //if (direction.magnitude >= 0.1f)
            //{
            //	float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            //	stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 180, 0f);


            //if (attackForceTime > 0)
            //{
            
            Vector3 forwardDir = new Vector3(stateMachine.mainCamera.forward.x, 0f, stateMachine.mainCamera.forward.z).normalized;
            Vector3 direction = new Vector3(-stateMachine.transform.position.x, 0f, -stateMachine.transform.position.y).normalized;
            direction = Quaternion.AngleAxis(stateMachine.mainCamera.rotation.eulerAngles.y, Vector3.up) * direction;

            if (direction.magnitude >= 0.1f)//Manipulate rootHip to face target
            {
                float targetAngle = Mathf.Atan2(forwardDir.z, forwardDir.x) * Mathf.Rad2Deg;

                stateMachine.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle - 180, 0f);
            }

            //Apply Enhanced Gravity Manually
//            for (int i = 0; i < stateMachine.allRigidbodies.Count; i++)
 //           {
   //             stateMachine.allRigidbodies[i].drag = stateMachine.newAttackDrag;
     //       }
            //stateMachine.hipJoint.targetRotation = Quaternion.LookRotation(stateMachine.mainCamera.position - stateMachine.transform.position);//Quaternion.Euler(0f, targetAngle - 180, 0f);
            //}
            //stateMachine.hip.velocity = Vector3.down * 8000f * Time.deltaTime;
            stateMachine.hip.AddForce(Vector3.up * Physics.gravity.y * (stateMachine.fallMulti - 1) * Time.deltaTime);
        }

        //Attack Transition
        public override void AttemptAttack(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            if (hasDamageBeenEnabled) { return; }

            /*
            if (damageEnabledTime <= 0)
            {
                //Queue next attack once this ones
                nextAttackQueued = true;
            }
            */
        }

        //Jump Transition
        public override void AttemptJump(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            if (hasDamageBeenEnabled) { return; }

            stateMachine.SwitchState(stateMachine.jumpState);
        }

        //DodgeTransition
        public override void AttemptDodge(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            if (hasDamageBeenEnabled) { return; }

            stateMachine.SwitchState(stateMachine.diveState);
        }

        //Ragdoll Transition
        public override void AttemptRagdoll(StateMachine stateMachine)
        {
            if (!stateMachine.view.IsMine)
            { return; }

            stateMachine.SwitchState(stateMachine.fullRagdollState);
        }
    }
}
