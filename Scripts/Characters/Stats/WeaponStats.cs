using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.Weapons;
using MainGame.Characters.BaseStats.Stat;

namespace MainGame.Characters.BaseStats
{
    public class WeaponStats : MonoBehaviour
    {
        [SerializeField] List<CharacterStat> CurrentStats;
        [SerializeField] WeaponInfoSO weaponBaseData;

        private void Awake()
        {
            //setup current stats
            UpdateStats();
        }

        public float getStatByString(string statName)
        {
            for (int i = 0; i < CurrentStats.Count; i++)
            {
                if (CurrentStats[i].GetName() == statName)
                {
                    return CurrentStats[i].GetVal();
                }
            }

            Debug.Log("STAT NAME INCCORRECT CHANGE STRING: " + statName);
            return -1;
        }

        public void addToStatByString(string statName, float value)
        {
            for (int i = 0; i < CurrentStats.Count; i++)
            {
                if (CurrentStats[i].GetName() == statName)
                {
                    CurrentStats[i].SetValue(value + CurrentStats[i].GetVal());
                    return;
                }
            }

            Debug.Log("STAT NAME INCCORRECT CHANGE STRING: " + statName);
        }

        public void decrStatByString(string statName, float value)
        {
            for (int i = 0; i < CurrentStats.Count; i++)
            {
                if (CurrentStats[i].GetName() == statName)
                {
                    CurrentStats[i].SetValue(CurrentStats[i].GetVal() - value);
                    return;
                }
            }

            Debug.Log("STAT NAME INCCORRECT CHANGE STRING: " + statName);
        }

        //warning to self when updating stats it  will remove all information recieved from relics. They will have to reapply their values to the weapon
        public void UpdateStats()
        {
            //setup current stats
            CurrentStats[0].SetValue(weaponBaseData.baseDamage);
            CurrentStats[1].SetValue(weaponBaseData.knockbackForce);
            CurrentStats[2].SetValue(weaponBaseData.poiseDisruption);
            CurrentStats[3].SetValue(weaponBaseData.baseDurability);

            //Refresh relic data additions

            //--
            //-
        }
    }
}