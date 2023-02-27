using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Utils;

namespace MainGame.Interactables
{
    //Makes the object attachable to any object and allows itself to be interacted with at some range
    public class InteractableBase : MonoBehaviour
    {
        [SerializeField] Transform interactPoint;
        [SerializeField] float interactRange;

        InputManager inputManager;
        [SerializeField] GameObject myPlayerRoot;

        private void Awake()
        {
            inputManager = InputManager.Instance;
            StartCoroutine(searchTillPresent());
        }

        private void OnEnable()
        {
            inputManager.OnStartInteract += IgnoreInteract;
        }

        private void OnDisable()
        {
            inputManager.OnStartInteract -= IgnoreInteract;
        }

        public void IgnoreInteract()
        {
            //Only interact if within range
            if (Vector3.Distance(interactPoint.position, myPlayerRoot.transform.position) > interactRange) { return; }
            PerformInteract();
        }

        //inheriting classes can setup their own interaction
        public virtual void PerformInteract() {}

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(interactPoint.position, interactRange);
        }

        private IEnumerator searchTillPresent()
        {
            while(AllPlayersHolder.Instance.myStateMachine == null)
            {
                yield return new WaitForEndOfFrame();
            }

            myPlayerRoot = AllPlayersHolder.Instance.myStateMachine.hip.gameObject;
        }
    }
}