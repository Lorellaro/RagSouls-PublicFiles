using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.LevelProgression.Rooms
{
    [System.Serializable]
    public class EnemyCharSpawnPos
    {
        public Transform spawnPos;
        public List<EnemyCharacterSpawnData> spawnObjs;
    }
}