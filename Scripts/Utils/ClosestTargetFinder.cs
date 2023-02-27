using UnityEngine;

namespace MainGame.Utils
{
	public class ClosestTargetFinder : MonoBehaviour
	{
		public static Transform GetClosestTarget(Transform myTransform, Transform[] enemies)
		{
			Transform bestTarget = null;
			float closestDistanceSqr = Mathf.Infinity;
			Vector3 currentPosition = myTransform.position;
			foreach (Transform potentialTarget in enemies)
			{
				Vector3 directionToTarget = potentialTarget.position - currentPosition;
				float dSqrToTarget = directionToTarget.sqrMagnitude;
				if (dSqrToTarget < closestDistanceSqr)
				{
					closestDistanceSqr = dSqrToTarget;
					bestTarget = potentialTarget;
				}
			}

			return bestTarget;
		}
	}
}
