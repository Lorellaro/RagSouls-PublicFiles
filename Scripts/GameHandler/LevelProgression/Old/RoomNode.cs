using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Map
{
    public enum NodeStates
    {
        Locked, 
        Visited,
        Attainable
    }
}

namespace MainGame.Map
{
    public class RoomNode : MonoBehaviour
    {
        public Node Node { get; private set; }
        public NodeBlueprint Blueprint { get; private set; }
    }
}
