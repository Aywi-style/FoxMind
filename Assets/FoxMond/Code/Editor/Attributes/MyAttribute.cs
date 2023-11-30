using System;
using UnityEditor;

namespace Fearness.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class MyAttribute : Attribute
    {
        public string Description { get; private set; }
        
        public MyAttribute(string description)
        {
            Description = description;
        }

        [MenuItem("Tools/Project Documentation")]
        private static void OpenDocs()
        {
            
        }
    }
}