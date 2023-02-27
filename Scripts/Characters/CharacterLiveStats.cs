using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.BaseStats;

public class CharacterLiveStats : MonoBehaviour
{
	[SerializeField] CharacterBaseStats charBaseStats;

	string _myName;
	float _health;
	float _poise;
	float _attackDamages;
	float _attackForces;
	float _attackPoiseDisruptor;

	private void Awake()
	{
		//Get base stats from scriptable object.
		_myName = charBaseStats.myName;
		_health = charBaseStats.health;
		_poise = charBaseStats.poise;
		_attackDamages = charBaseStats.attackDamage;
		_attackDamages = charBaseStats.attackKnockback;
		_attackPoiseDisruptor = charBaseStats.PoiseDisruptor;
	}


}
