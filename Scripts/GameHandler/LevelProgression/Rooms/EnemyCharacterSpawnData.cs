using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.LevelProgression.Rooms
{
    [System.Serializable]
    public class EnemyCharacterSpawnData
    {
        //spawn position

        public GameObject potentialEnemyToBeSpawnedHere;// pair each potential enemy with a probability
        [Range(0, 100)] public float spawnProbability;
    }
}