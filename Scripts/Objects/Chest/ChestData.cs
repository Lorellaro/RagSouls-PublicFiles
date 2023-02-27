using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Interactables.Chest;

namespace MainGame.Interactables.Chest
{
    [CreateAssetMenu(fileName = "Chest Data", menuName = "Chest info")]
    public class ChestData : ScriptableObject
    {
        [Header("Relic Info")]
        [Range(0, 100)] public int doubleRelicChance;
        [Range(0, 100)] public int RareRelicChance;
        [Range(0, 100)] public int LegendaryRelicChance;

        [Header("Gold Info")]
        //Gold bursts handle particle system emissions of the gold
        [Range(5, 20)] public int goldPerBurst;
        [Range(1, 3)] public int goldBursts;

        //Handles the random variance of gold per burst
        [Range(-10, 10)] public int randomGoldVarianceValueMin;
        [Range(-10, 10)] public int randomGoldVarianceValueMax;

        [Header("Chest Rarity")]
        [SerializeField] public rarity _rarity;

        public enum rarity
        {
            Common, Rare, Legendary
        }
    }
}