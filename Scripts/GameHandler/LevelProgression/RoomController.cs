using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.LevelProgression.Rooms
{
    public class RoomController : MonoBehaviour
    {
        RoomBase currentRoom;

        public void GenerateRoom(RoomBase _currentRoom)
        {
            currentRoom = _currentRoom;
        }
    }
}
