using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MainGame.Map
{
    public class Node
    {
        public Point point;
        public List<Point> incoming = new List<Point>();
        public List<Point> outgoing = new List<Point>();
        public NodeType nodeType;
        public string blueprintName;
        public Vector2 position;

        public Node(NodeType nodeType, string blueprintName, Point point)
        {
            this.nodeType = nodeType;
            this.blueprintName = blueprintName;
            this.point = point;
        }

        //If not present add it
        public void AddIncoming(Point p)
        {
            if(incoming.Any(element => element.Equals(p))) { return; }

            incoming.Add(p);

        }

        //if not present add it
        public void AddOutgoing(Point p)
        {
            if(outgoing.Any(element => element.Equals(p))) { return; }

            outgoing.Add(p);
        }

        public void RemoveIncoming(Point p)
        {
            incoming.RemoveAll(element => element.Equals(p));
        }

        public void RemoveOutgoing(Point p)
        {
            outgoing.RemoveAll(element => element.Equals(p));
        }

        public bool HasNoConnections()
        {
            return incoming.Count == 0 && outgoing.Count == 0;
        }
    }
}