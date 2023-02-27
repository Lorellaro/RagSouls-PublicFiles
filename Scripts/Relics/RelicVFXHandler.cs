using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Relics;

namespace MainGame.Relics
{
    public class RelicVFXHandler : MonoBehaviour
    {
        [SerializeField] GameObject relicPrefab;
        [SerializeField] GameObject floorCollisionSFX;
        [SerializeField] public ParticleSystem particleSystem;
        [SerializeField] List<ParticleCollisionEvent> collisionEvents;

        GiveRandomAudioClip randomAudioClip;

        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
            randomAudioClip = GetComponent<GiveRandomAudioClip>();
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);

            Instantiate(relicPrefab, collisionEvents[0].intersection, Quaternion.identity);

            Instantiate(floorCollisionSFX, collisionEvents[0].intersection, Quaternion.identity);
        }

        public void Play()
        {
            //play sfx
            randomAudioClip.changeAndPlayClip();
            particleSystem.Play();
        }

        public void SetChestRelicContents(GameObject _relicPrefab)
        {
            relicPrefab = _relicPrefab;
        }
    }
}