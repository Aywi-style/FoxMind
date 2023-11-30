using Unity.Entities;
using UnityEngine;

namespace Fearness.Code.Testing
{
    public class EntityTemplateAuthoring : MonoBehaviour
    {
        
    }
    
    public class EntityTemplateBaker : Baker<EntityTemplateAuthoring>
    {
        public override void Bake(EntityTemplateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
        }
    }
}