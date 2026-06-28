using FoxMind.Code.Runtime.Core.Stats.Features;
using KinematicCharacterController;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Movement.KinematicCharacterBehaviours
{
    public class DashBehaviour
    {
        private KinematicCharacterMotor _motor;

        [SerializeField] private float _dashPower;
        
        public void Initialize(KinematicCharacterMotor motor)
        {
            _motor = motor;
        }

        public void SetDashRequest(Vector3 dashDirection, UnitStatsComp unitStats)
        {
            _motor.SetPosition(_motor.TransientPosition + dashDirection * _dashPower, bypassInterpolation: true);
        }
    }
}