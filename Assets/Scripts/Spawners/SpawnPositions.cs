using UnityEngine;

namespace Spawners
{
    public class SpawnPositions : ScriptableObject
    {
        public Vector3[] Positions;
    }

    public class PointPosition
    {
        public Vector3 Position;
        public int Distance;
    }
}