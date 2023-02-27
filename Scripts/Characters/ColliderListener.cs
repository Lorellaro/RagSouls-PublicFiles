using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Characters.States;
using Player.States;

public class ColliderListener : MonoBehaviour
{
    [SerializeField] bool isEnemy = true;
    [SerializeField] bool isPlayer;

	EnemyStateController enemyStateController;
    StateMachine playerStateController;

	void Awake()
	{
		// Check if Colider is in another GameObject
		Collider[] colliders = GetComponentsInChildren<Collider>();

		for(int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				ColliderBridge cb = colliders[i].gameObject.AddComponent<ColliderBridge>();
				cb.Initialize(this);
			}
		}

        if (isEnemy)
        {
            enemyStateController = GetComponent<EnemyStateController>();
        }

        if (isPlayer)
        {
            playerStateController = GetComponent<StateMachine>();
        }
    }

	public void OnCollisionEnter(Collision collision)
	{
        if (isEnemy)
        {
            enemyStateController.CollisionEnter(collision);
        }

        if (isPlayer)
        {
            playerStateController.playerCollisionEnter(collision);
        }
    }
	/*
	public void OnTriggerEnter2D(Collider2D other)
	{
		// Do your stuff here
	}
	*/
}
