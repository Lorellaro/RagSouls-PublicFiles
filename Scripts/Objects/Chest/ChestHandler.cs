using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Utils;
using MainGame.Relics;
using Photon.Pun;

namespace MainGame.Interactables.Chest
{
    public class ChestHandler : MonoBehaviour
    {
        //Contents change based on chest's rarity
        //Randomly selects between x to y amount of relics, weapons, and potions - add potions in later
        //manipulates particle system 

        [SerializeField] float interactRange;
        [SerializeField] GameObject chestGlowObj;
        [SerializeField] AllRelics allRelicData;
        [SerializeField] ChestData chestData;
		[SerializeField] AudioSource chestOpenSFX;
        [SerializeField] RelicVFXHandler relicVFXHandler;
        [SerializeField] ParticleSystem chestOpenGoldVFX;

        Animator anim;
        InputManager inputManager;
        Transform myPlayerTransform;
        PhotonView view;

        bool opened;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            inputManager = InputManager.Instance;
            myPlayerTransform = AllPlayersHolder.Instance.myStateMachine.hip.transform;
            view = GetComponent<PhotonView>();

            //Setup gold emission according to chest data

            //Randomise amount of Gold in chest
            int randomGoldPerBurst = chestData.goldPerBurst + Random.Range(chestData.randomGoldVarianceValueMin, chestData.randomGoldVarianceValueMax + 1);

            ParticleSystem.Burst burst = chestOpenGoldVFX.emission.GetBurst(0);
            burst.count = randomGoldPerBurst;
            burst.cycleCount = chestData.goldBursts;
            chestOpenGoldVFX.emission.SetBurst(0, burst);


            // ------  -------  Randomise relic output

            // Randomly selects how many relics
            int randomPercent = Random.Range(0, 101);

            //Spawns 2 if percent is in range
            if(randomPercent < chestData.doubleRelicChance)
            {
                ParticleSystem.Burst relicBurst = relicVFXHandler.particleSystem.emission.GetBurst(0);
                relicBurst.cycleCount = 2;
                relicVFXHandler.particleSystem.emission.SetBurst(0, relicBurst);
            }

            //Randomly Selects Relic rarity
            int randomRarityPercent = Random.Range(0, 101);
            GameObject randomisedRelic = null;

            if (randomRarityPercent <= chestData.LegendaryRelicChance)
            {
                //Legendary relic earned
                int randomRelicIndex = Random.Range(0, allRelicData.allLegendaryRelics.Count);
                randomisedRelic = allRelicData.allLegendaryRelics[randomRelicIndex].relicPrefab;
            }
            else if(randomRarityPercent <= chestData.RareRelicChance)
            {
                //Rare relic earned
                int randomRelicIndex = Random.Range(0, allRelicData.allRareRelics.Count);
                randomisedRelic = allRelicData.allRareRelics[randomRelicIndex].relicPrefab;
            }
            else
            {
                //Common relic earned
                int randomRelicIndex = Random.Range(0, allRelicData.allCommonRelics.Count);
                randomisedRelic = allRelicData.allCommonRelics[randomRelicIndex].relicPrefab;
            }

            relicVFXHandler.SetChestRelicContents(randomisedRelic);
        }

        private void OnEnable()
        {
            inputManager.OnStartInteract += ChestInteract;
        }

        private void OnDisable()
        {
            inputManager.OnStartInteract -= ChestInteract;
        }

        public void ChestInteract()
        {
            if (opened) { return; }

            if(Vector3.Distance(myPlayerTransform.position, transform.position) < interactRange)
            {
                view.RPC("openChest", RpcTarget.AllBuffered);
                //Shoot out gold relic and other contents
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, interactRange);
        }

        [PunRPC]
        public void openChest()
        {
            anim.Play("Open");
			GetComponent<GiveRandomAudioClip>().changeAndPlayClip();
            opened = true;
            chestGlowObj.SetActive(false);
            StartCoroutine(waitThenPlayVFX());
        }

        private IEnumerator waitThenPlayVFX()
        {
            yield return new WaitForSeconds(0.15f);
            chestOpenGoldVFX.Play();
            chestOpenGoldVFX.GetComponent<GiveRandomAudioClip>().changeAndPlayClip();
            relicVFXHandler.Play();
        }
    }
}
