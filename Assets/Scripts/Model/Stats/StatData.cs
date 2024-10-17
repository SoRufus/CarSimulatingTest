using System;
using UnityEngine;

namespace Model.Stats
{
    [Serializable]
    public class StatData
    {
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        
        public void SetDefaultValue()
        {
            Value = BaseValue;
        }
        
        public void SetValue(float value)
        {
            Value = value;
        }
    }
}