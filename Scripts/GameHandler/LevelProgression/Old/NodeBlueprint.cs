using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Map;

namespace MainGame.Map
{
    //All room types
    public enum NodeType
    {
        StandardBattle,
        ChallengingBattle,
        Merchant,
        FreeReward,
        RestSite,
        Mystery,
        Boss
    }
}

namespace MainGame.Map
{
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        //sprite?
        public Sprite sprite;
        public List<GameObject> nodeRoomPrefabs;
        public NodeType nodeType;
    }
}
