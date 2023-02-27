using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Map
{
    public class MapManager : MonoBehaviour
    {
        public MapConfig mapConfig;
        public int currentLayerIndex;
        public Map currentMap { get; private set; }

        private void Start()
        {
            
        }

        public void GenerateNewMap()
        {
            var map = MapGenerator.GetMap(mapConfig);
            currentMap = map;
        }
    }
}