using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MainGame.Utils;

namespace MainGame.GameHandling
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public AudioSource musicPlayerTEMPORARY;

        InputManager inputManager;

        public float minX;
        public float maxX;
        public float minY;
        public float maxY;
        public float minZ;
        public float maxZ;

        bool SpawnIsInRangeTEMPORARY;
        bool hasStartedMusic;

        private void Awake()
        {
            if (inputManager == null)
            {
                inputManager = InputManager.Instance;
            }
        }

        private void OnEnable()
        {
            inputManager.OnStartInteract += SpawnEnemyRandomly;
        }

        private void OnDisable()
        {
            inputManager.OnStartInteract -= SpawnEnemyRandomly;
        }

        public void SpawnEnemy(Vector3 _position)
        {
            PhotonNetwork.Instantiate(enemyPrefab.name, _position, Quaternion.identity);
        }

        public void SpawnEnemyRandomly()
        {
            if (!SpawnIsInRangeTEMPORARY) { return; }
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            PhotonNetwork.Instantiate(enemyPrefab.name, randomPos, Quaternion.identity);

            //TEMPORARY NEEDS TO BE REWORKED WITH PROPER MUSIC PLAYER INTERACTION
            if (hasStartedMusic) { return; }
            if(musicPlayerTEMPORARY == null) { return; }
            musicPlayerTEMPORARY.Play();
            hasStartedMusic = true;
        }

        private void OnTriggerEnter(Collider collision)
        {
            //TEMPORARY ALLOWS FOR SPAWNING ONLY WHEN WITHIN RADIUS
            SpawnIsInRangeTEMPORARY = true;
        }

        private void OnTriggerExit(Collider collision)
        {
            SpawnIsInRangeTEMPORARY = false;
        }
    }
}