using System.Collections.Generic;
using FoxMind.Code.Runtime.Core.Animations.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Components;
using FoxMind.Code.Runtime.Core.Battle.Combo.Enums;
using FoxMind.Code.Runtime.Core.Ecs.SystemsAssembly.Abstracts;
using FoxMind.Code.Runtime.Core.Input.Components;
using FoxMind.Code.Runtime.Core.PlayerActions.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.Combo.Systems
{
    public class AttackSystem : BaseEcsVisitable, IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorldInject _world = default;
        private readonly EcsFilterInject<Inc<BaseInputControlsComp>> _baseInputFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControlledComp, CombinableComp, AnimancerComp>> _comboFilter = default;

        private readonly EcsPoolInject<CombinableComp> _combosPool = default;
        private readonly EcsPoolInject<AnimancerComp> _animancerPool = default;

        private Dictionary<ComboCondition, IEcsPool>  _inputsPools;

        public void Init(IEcsSystems systems)
        {
            _inputsPools = new Dictionary<ComboCondition, IEcsPool>()
                {
                    {
                        ComboCondition.Attack, _world.Value.GetPool<InputAttackEvent>()
                    },
                    {
                        ComboCondition.Dash, _world.Value.GetPool<InputDashEvent>()
                    },
                    {
                        ComboCondition.Jump, _world.Value.GetPool<InputJumpEvent>()
                    },
                    {
                        ComboCondition.ForwardMove, _world.Value.GetPool<InputAttackEvent>()
                    },
                    {
                        ComboCondition.BackwardMove, _world.Value.GetPool<InputAttackEvent>()
                    },
                    {
                        ComboCondition.LeftMove, _world.Value.GetPool<InputAttackEvent>()
                    },
                    {
                        ComboCondition.RightMove, _world.Value.GetPool<InputAttackEvent>()
                    },
                }
                ;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var baseInput in _baseInputFilter.Value)
            {
                foreach (var combinableEntity in _comboFilter.Value)
                {
                    ref var combinableComp = ref _combosPool.Value.Get(combinableEntity);

                    foreach (var comboConfig in combinableComp.CombosAssembly.ComboConfigs)
                    {
                        bool isActualCombo = true;
                        
                        foreach (var comboCondition in comboConfig.ComboConditions)
                        {
                            if (_inputsPools.TryGetValue(comboCondition, out var pool) == false)
                            {
                                Debug.LogError($"Пул для {comboCondition} не существует");
                                isActualCombo = false;
                                break;
                            }

                            if (pool.Has(baseInput) == false)
                            {
                                isActualCombo = false;
                                break;
                            }
                        }

                        if (isActualCombo == false)
                        {
                            continue;
                        }

                        ref var animancerComp = ref _animancerPool.Value.Get(combinableEntity);
                        
                        animancerComp.Value.Animator.applyRootMotion = true;
                        animancerComp.Value.Play(comboConfig.AttackConfig.Animation).Time = 0;
                    }
                }
            }
        }
    }
}