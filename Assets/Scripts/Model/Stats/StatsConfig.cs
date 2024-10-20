using UnityEngine;

namespace Model.Stats
{
    [CreateAssetMenu(fileName = nameof(StatsConfig), menuName = "Configs/StatsConfig")]
    public class StatsConfig : ScriptableObject
    {
        [field: SerializeField] public StatData MaxSpeed { get; private set; }
        [field: SerializeField] public StatData MinSpeed { get; private set; }
        [field: SerializeField] public StatData Acceleration { get; private set; }
        [field: SerializeField] public StatData Deceleration { get; private set; }
        [field: SerializeField] public StatData DecelerationOnCrossRoads { get; private set; }

        private void OnEnable()
        {
            SetDefaultValues();
        }
        
        private void OnDisable()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            MaxSpeed.SetDefaultValue();
            Acceleration.SetDefaultValue();
            Deceleration.SetDefaultValue();
            DecelerationOnCrossRoads.SetDefaultValue();
            MinSpeed.SetDefaultValue();
        }
    }
}
