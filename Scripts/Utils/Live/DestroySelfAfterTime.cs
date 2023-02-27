using UnityEngine;

namespace MainGame.Utils
{
	public class DestroySelfAfterTime : MonoBehaviour
	{
		[SerializeField] private float timeTillDestroy;

		void Start()
		{
			Destroy(gameObject, timeTillDestroy);
		}
	}
}
