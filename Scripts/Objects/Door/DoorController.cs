using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Interactables.Door
{
    public class DoorController : InteractableBase
    {
        [SerializeField] Animator myAnim;

        bool hasInteracted;

        public override void PerformInteract()
        {
            //Checks if is in range to interact
            base.PerformInteract();

            if (hasInteracted) { return; }

            myAnim.Play("Open");
            hasInteracted = true;
            GetComponent<GiveRandomAudioClip>().changeAndPlayClip();
        }
    }
}
