using System;
using FoxMind.Code.Runtime.Core.Fractions.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Fractions.Configs
{
    [CreateAssetMenu(fileName = "FractionsRelationships", menuName = "Configs/FractionsRelationships", order = 0)]
    public class FractionsRelationships : SerializedScriptableObject
    {
        public const string Fractions = nameof(FractionEnum.Neutral) + ", " + nameof(FractionEnum.Villager) + ", " + nameof(FractionEnum.Corporate);
        public const string RevertFractions = nameof(FractionEnum.Corporate) + ", " + nameof(FractionEnum.Villager) + ", " + nameof(FractionEnum.Neutral);
        
        [TableMatrix(HorizontalTitle = Fractions, VerticalTitle = RevertFractions, SquareCells = false)]
        public float[,] Relationships;
    }
}