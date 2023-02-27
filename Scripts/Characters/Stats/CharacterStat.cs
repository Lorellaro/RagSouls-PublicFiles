using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MainGame.Characters.BaseStats.Stat
{
	[System.Serializable]
	public class CharacterStat
	{
		[SerializeField] string Name;
		[SerializeField] float Value;

		public void SetValue(float _Val)
		{
			Value = _Val;
		}

		public void IncreaseValue(float _ValToAdd)
		{
			Value += _ValToAdd;
		}

		public void DecreaseValue(float _ValToDecr)
		{
			Value += _ValToDecr;
		}

		public void IncreaseForTime(MonoBehaviour mono, float duration, float _ValToAdd)
		{
			mono.StartCoroutine(IncreaseForTimeFunctionality(duration, _ValToAdd));
		}

		private IEnumerator IncreaseForTimeFunctionality(float duration, float _ValToAdd)
		{
			Value += _ValToAdd;
			yield return new WaitForSeconds(duration);
			Value -= _ValToAdd;
		}

		public void DecreaseForTime(MonoBehaviour mono, float duration, float _ValToDecr)
		{
			mono.StartCoroutine(DecreaseForTimeFunctionality(duration, _ValToDecr));
		}

		private IEnumerator DecreaseForTimeFunctionality(float duration, float _ValToDecr)
		{
			Value += _ValToDecr;
			yield return new WaitForSeconds(duration);
			Value -= _ValToDecr;
		}

		public float GetVal()
		{
			return Value;
		}

		public string GetName()
		{
			return Name;
		}
	}
}
