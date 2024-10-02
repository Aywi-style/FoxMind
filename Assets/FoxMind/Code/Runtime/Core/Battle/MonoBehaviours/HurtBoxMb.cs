using System;
using FoxMind.Code.Runtime.Core.Ecs.MonoBehaviours;
using Leopotam.EcsLite;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Battle.MonoBehaviours
{
    public class HurtBoxMb : MonoBehaviour
    {
        [SerializeField] private EntityBaker _entityBaker;

        public EcsPackedEntityWithWorld PackedEntity => _entityBaker.PackedEntity;

        
    }
}