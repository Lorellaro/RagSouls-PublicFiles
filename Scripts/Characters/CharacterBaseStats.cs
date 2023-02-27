using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Characters.BaseStats
{
	[CreateAssetMenu(fileName = "Base Stats", menuName = "Stats")]
	public class CharacterBaseStats : ScriptableObject
	{
		[SerializeField] public string myName;
		[SerializeField] public float health;
		[SerializeField] public float defense;
		[SerializeField] public float poise;
		[SerializeField] public float weight;
		[SerializeField] public float movementSpeed;
		[SerializeField] public float jumpHeight;
		[Header("Attack Details")]
		[SerializeField] public float attackDamage;
		[SerializeField] public float attackKnockback;
		[SerializeField] public float PoiseDisruptor;
	}
}
