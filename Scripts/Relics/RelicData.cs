using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Relics
{
    [CreateAssetMenu(fileName = "Relic Data", menuName = "Relic info")]
    public class RelicData : ScriptableObject
    {
        [SerializeField] public string relicName;
        [TextArea(15, 20)]
        [SerializeField] public string relicDescription;
        [SerializeField] public string relicDescriptionShort;
        [SerializeField] public bool isEffectGood = true;
        [SerializeField] public Rarity rarity;
        [SerializeField] public GameObject relicPrefab;
        [SerializeField] Relic Reliczino;

    }

    public enum Rarity
    {
        Common, Rare, Legendary
    }
}