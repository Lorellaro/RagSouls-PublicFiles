using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Relics;

namespace MainGame.Relics
{
    [CreateAssetMenu(fileName = "Relic List", menuName = "Relic List")]
    public class AllRelics : ScriptableObject
    {
        public List<RelicData> allRelicData;
        public List<RelicData> allCommonRelics;
        public List<RelicData> allRareRelics;
        public List<RelicData> allLegendaryRelics;
    }
}