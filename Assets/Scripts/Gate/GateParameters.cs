using UnityEngine;

namespace Gate
{
    [CreateAssetMenu(fileName = "Gate Parameter", menuName = "ScriptableObjects/Gate Parameter", order = 1)]
    public class GateParameters : ScriptableObject
    {
        public MultiplyType MultiplyType;
        public int Value;
    }
}