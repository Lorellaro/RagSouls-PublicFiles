using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.AttackData;

namespace MainGame.Characters.Weapons
{
	[CreateAssetMenu(fileName = "Weapon Data", menuName = "Weapon Info")]
	public class WeaponInfoSO : ScriptableObject
	{
		public string weaponName;
		public float baseDamage;
		public float force;
		public float knockbackForce;
		public float poiseDisruption;
		public float baseDurability;
		public float baseAttackSpeed = 1;
		public float weaponMass;
		[Tooltip("Only counts if attack is landed")] public float attackDurabilityCost;
		public GameObject WeaponRightHandPrefab;
		public GameObject WeaponLeftHandPrefab;
		public WeaponHandling weaponHanding;
		public WeaponOrigin weaponOrigin;
		public Rarity rarity;
		public List<AttackDataSO> weaponAttackData;

		public enum WeaponHandling
		{
			TwoHanded, DualWield, RightHander, LeftHander
		}

		public enum WeaponOrigin
		{
			Eastern, Western, Northern, Southern
		}

		public enum Rarity
		{
			GigaLegendary, Legendary, Epic, Rare, UnCommon, Common
		}
	}
}
