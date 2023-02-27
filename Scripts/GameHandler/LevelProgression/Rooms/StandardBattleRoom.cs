using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.LevelProgression.Rooms
{
    public class StandardBattleRoom : RoomBase
    {
        [SerializeField] List<Transform> enemySpawnPositions;
        [SerializeField] List<GameObject> enemySpawnObjects;
        [SerializeField] List<EnemyCharSpawnPos> enemyCharSpawnData;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
