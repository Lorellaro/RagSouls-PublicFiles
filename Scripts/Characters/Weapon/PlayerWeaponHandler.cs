using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.Weapons;
using Photon.Pun;

namespace MainGame.Characters.Weapons
{
	public class PlayerWeaponHandler : MonoBehaviour
	{
		[SerializeField] WeaponInfoSO currentWeapon;
		[SerializeField] Transform R_Hand;
		[SerializeField] Transform L_Hand;
		[SerializeField] public GameObject active_R_Weapon;
		[SerializeField] public GameObject active_L_Weapon;

		PhotonView view;

		private void Start()
		{
			view = transform.root.GetComponent<PhotonView>();
			updateWeapon();
		}

		public void updateWeapon()
		{
			//change for me
			if (view.IsMine)
			{
				changeWeaponCosmetic();
			}

			//notify other players
		}

		private void changeWeaponCosmetic()
		{
			//destroy current weapons(if the exist)

			//instantiate and attach new weapons

			attachWeapon();
		}

		private void attachWeapon()
		{
			//check how current weapon will be held
			switch (currentWeapon.weaponHanding)
			{
				case WeaponInfoSO.WeaponHandling.DualWield:
					print("Not yet implemented");
					break;

				case WeaponInfoSO.WeaponHandling.TwoHanded:
					print("Not yet implemented");
					break;

				case WeaponInfoSO.WeaponHandling.RightHander:
					print("Not yet implemented");
					break;

				case WeaponInfoSO.WeaponHandling.LeftHander:
					print("Not yet implemented");
					break;
			} 
		}
	}
}
