using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.LevelProgression.Rooms
{
    public class RoomData : ScriptableObject
    {
        public roomType roomType;


    }

    public enum roomType
    {
        StandardBattle,
        ChallengingBattle,
        Merchant,
        FreeReward,
        RestSite,
        Mystery,
        Boss
    }
}