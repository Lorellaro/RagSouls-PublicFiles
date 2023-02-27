using UnityEngine;

namespace MainGame.Map
{
    [System.Serializable]
    public class LineConnection
    {
        public LineRenderer lr;
        public RoomNode from;
        public RoomNode to;

        public LineConnection(LineRenderer lr, RoomNode from, RoomNode to)
        {
            this.lr = lr;
            this.from = from;
            this.to = to;
        }

        public void SetColor(Color color)
        {
            // Debug.Log("In setcolor");
            // lr.material.color = color;

            var gradient = lr.colorGradient;
            var colorKeys = gradient.colorKeys;
            for (var j = 0; j < colorKeys.Length; j++)
            {
                colorKeys[j].color = color;
            }

            gradient.colorKeys = colorKeys;
            lr.colorGradient = gradient;
        }
    }
}