using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.BaseStats.Stat;
using Photon.Pun;

namespace MainGame.Characters.BaseStats
{
	public class LiveStats : MonoBehaviour
	{
		[SerializeField] List<CharacterStat> CurrentStats;
		[SerializeField] CharacterBaseStats thisCharacterBaseStats;
		[SerializeField] public PhotonView view;

		private void Awake()
		{
			//setup current stats
			CurrentStats[0].SetValue(thisCharacterBaseStats.health);
			CurrentStats[1].SetValue(thisCharacterBaseStats.defense);
			CurrentStats[2].SetValue(thisCharacterBaseStats.poise);
			CurrentStats[3].SetValue(thisCharacterBaseStats.weight);
			CurrentStats[4].SetValue(thisCharacterBaseStats.movementSpeed);
			CurrentStats[5].SetValue(thisCharacterBaseStats.jumpHeight);
			CurrentStats[6].SetValue(thisCharacterBaseStats.attackDamage);
			CurrentStats[7].SetValue(thisCharacterBaseStats.attackKnockback);
			CurrentStats[8].SetValue(thisCharacterBaseStats.PoiseDisruptor);
		}

		public float getStatByString(string statName)
		{
			for (int i = 0; i < CurrentStats.Count; i++)
			{
				if(CurrentStats[i].GetName() == statName)
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
    }
}
