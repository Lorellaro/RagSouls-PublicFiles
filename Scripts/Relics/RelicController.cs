using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame.Relics;

namespace MainGame.Relics
{
    public class RelicController : Singleton<RelicController>
    {
        [SerializeField] public List<Relic> ActiveRelicsList;

        public void AddRelic(Relic _relic)
        {
            ActiveRelicsList.Add(_relic);
            _relic.RelicStart();
        }

        private void OnDisable()
        {
            for(int i = 0; i < ActiveRelicsList.Count; i++)
            {
                ActiveRelicsList[i].RelicEnd();
            }
        }
    }
}
