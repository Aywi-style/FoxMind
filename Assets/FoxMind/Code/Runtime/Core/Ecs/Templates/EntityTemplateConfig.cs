using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs.Templates
{
    [CreateAssetMenu(fileName = "EntityTemplateConfig", menuName = "Configs/EntityTemplate")]
    [InlineEditor]
    public class EntityTemplateConfig : ScriptableObject, IEnumerable<IEntityFeature>
    {
        [field: SerializeField] public string Name { private set; get; }
        
        [SerializeReference] private List<IEntityFeature> _features = new List<IEntityFeature>();
        
        public IEnumerator<IEntityFeature> GetEnumerator()
        {
            return _features.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}