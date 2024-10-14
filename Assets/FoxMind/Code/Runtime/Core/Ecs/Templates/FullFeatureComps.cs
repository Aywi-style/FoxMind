using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ecs.Templates
{
    public class FullFeatureComps<T1> : IEntityFeature
        where T1 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
    }
    
    public class FullFeatureComps<T1, T2> : IEntityFeature
        where T1 : IEntityFeature
        where T2 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        [Title("$ValueName2", bold: false), HideLabel, ShowInInspector] public T2 Value2;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
            Value2.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
        private string ValueName2() => Value2.GetType().Name;
    }
    
    public class FullFeatureComps<T1, T2, T3> : IEntityFeature
        where T1 : IEntityFeature
        where T2 : IEntityFeature
        where T3 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        [Title("$ValueName2", bold: false), HideLabel, ShowInInspector] public T2 Value2;
        [Title("$ValueName3", bold: false), HideLabel, ShowInInspector] public T3 Value3;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
            Value2.Compose(world, entity);
            Value3.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
        private string ValueName2() => Value2.GetType().Name;
        private string ValueName3() => Value3.GetType().Name;
    }
    
    public class FullFeatureComps<T1, T2, T3, T4> : IEntityFeature
        where T1 : IEntityFeature
        where T2 : IEntityFeature
        where T3 : IEntityFeature
        where T4 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        [Title("$ValueName2", bold: false), HideLabel, ShowInInspector] public T2 Value2;
        [Title("$ValueName3", bold: false), HideLabel, ShowInInspector] public T3 Value3;
        [Title("$ValueName4", bold: false), HideLabel, ShowInInspector] public T4 Value4;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
            Value2.Compose(world, entity);
            Value3.Compose(world, entity);
            Value4.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
        private string ValueName2() => Value2.GetType().Name;
        private string ValueName3() => Value3.GetType().Name;
        private string ValueName4() => Value4.GetType().Name;
    }
    
    public class FullFeatureComps<T1, T2, T3, T4, T5> : IEntityFeature
        where T1 : IEntityFeature
        where T2 : IEntityFeature
        where T3 : IEntityFeature
        where T4 : IEntityFeature
        where T5 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        [Title("$ValueName2", bold: false), HideLabel, ShowInInspector] public T2 Value2;
        [Title("$ValueName3", bold: false), HideLabel, ShowInInspector] public T3 Value3;
        [Title("$ValueName4", bold: false), HideLabel, ShowInInspector] public T4 Value4;
        [Title("$ValueName5", bold: false), HideLabel, ShowInInspector] public T5 Value5;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
            Value2.Compose(world, entity);
            Value3.Compose(world, entity);
            Value4.Compose(world, entity);
            Value5.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
        private string ValueName2() => Value2.GetType().Name;
        private string ValueName3() => Value3.GetType().Name;
        private string ValueName4() => Value4.GetType().Name;
        private string ValueName5() => Value5.GetType().Name;
    }
    
    public class FullFeatureComps<T1, T2, T3, T4, T5, T6> : IEntityFeature
        where T1 : IEntityFeature
        where T2 : IEntityFeature
        where T3 : IEntityFeature
        where T4 : IEntityFeature
        where T5 : IEntityFeature
        where T6 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        [Title("$ValueName2", bold: false), HideLabel, ShowInInspector] public T2 Value2;
        [Title("$ValueName3", bold: false), HideLabel, ShowInInspector] public T3 Value3;
        [Title("$ValueName4", bold: false), HideLabel, ShowInInspector] public T4 Value4;
        [Title("$ValueName5", bold: false), HideLabel, ShowInInspector] public T5 Value5;
        [Title("$ValueName6", bold: false), HideLabel, ShowInInspector] public T6 Value6;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
            Value2.Compose(world, entity);
            Value3.Compose(world, entity);
            Value4.Compose(world, entity);
            Value5.Compose(world, entity);
            Value6.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
        private string ValueName2() => Value2.GetType().Name;
        private string ValueName3() => Value3.GetType().Name;
        private string ValueName4() => Value4.GetType().Name;
        private string ValueName5() => Value5.GetType().Name;
        private string ValueName6() => Value6.GetType().Name;
    }
    
    public class FullFeatureComps<T1, T2, T3, T4, T5, T6, T7> : IEntityFeature
        where T1 : IEntityFeature
        where T2 : IEntityFeature
        where T3 : IEntityFeature
        where T4 : IEntityFeature
        where T5 : IEntityFeature
        where T6 : IEntityFeature
        where T7 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        [Title("$ValueName2", bold: false), HideLabel, ShowInInspector] public T2 Value2;
        [Title("$ValueName3", bold: false), HideLabel, ShowInInspector] public T3 Value3;
        [Title("$ValueName4", bold: false), HideLabel, ShowInInspector] public T4 Value4;
        [Title("$ValueName5", bold: false), HideLabel, ShowInInspector] public T5 Value5;
        [Title("$ValueName6", bold: false), HideLabel, ShowInInspector] public T6 Value6;
        [Title("$ValueName7", bold: false), HideLabel, ShowInInspector] public T7 Value7;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
            Value2.Compose(world, entity);
            Value3.Compose(world, entity);
            Value4.Compose(world, entity);
            Value5.Compose(world, entity);
            Value6.Compose(world, entity);
            Value7.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
        private string ValueName2() => Value2.GetType().Name;
        private string ValueName3() => Value3.GetType().Name;
        private string ValueName4() => Value4.GetType().Name;
        private string ValueName5() => Value5.GetType().Name;
        private string ValueName6() => Value6.GetType().Name;
        private string ValueName7() => Value7.GetType().Name;
    }
    
    public class FullFeatureComps<T1, T2, T3, T4, T5, T6, T7, T8> : IEntityFeature
        where T1 : IEntityFeature
        where T2 : IEntityFeature
        where T3 : IEntityFeature
        where T4 : IEntityFeature
        where T5 : IEntityFeature
        where T6 : IEntityFeature
        where T7 : IEntityFeature
        where T8 : IEntityFeature
    {
        [Title("$ValueName1", bold: false), HideLabel, ShowInInspector] public T1 Value1;
        [Title("$ValueName2", bold: false), HideLabel, ShowInInspector] public T2 Value2;
        [Title("$ValueName3", bold: false), HideLabel, ShowInInspector] public T3 Value3;
        [Title("$ValueName4", bold: false), HideLabel, ShowInInspector] public T4 Value4;
        [Title("$ValueName5", bold: false), HideLabel, ShowInInspector] public T5 Value5;
        [Title("$ValueName6", bold: false), HideLabel, ShowInInspector] public T6 Value6;
        [Title("$ValueName7", bold: false), HideLabel, ShowInInspector] public T7 Value7;
        [Title("$ValueName8", bold: false), HideLabel, ShowInInspector] public T8 Value8;
        
        public void Compose(EcsWorld world, int entity)
        {
            Value1.Compose(world, entity);
            Value2.Compose(world, entity);
            Value3.Compose(world, entity);
            Value4.Compose(world, entity);
            Value5.Compose(world, entity);
            Value6.Compose(world, entity);
            Value7.Compose(world, entity);
            Value8.Compose(world, entity);
        }

        private string ValueName1() => Value1.GetType().Name;
        private string ValueName2() => Value2.GetType().Name;
        private string ValueName3() => Value3.GetType().Name;
        private string ValueName4() => Value4.GetType().Name;
        private string ValueName5() => Value5.GetType().Name;
        private string ValueName6() => Value6.GetType().Name;
        private string ValueName7() => Value7.GetType().Name;
        private string ValueName8() => Value8.GetType().Name;
    }
}