using System.Reflection;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FoxMind.Code.Runtime.Core.Ecs.Aspects
{
    public static class EcsAspectInject
    {
        public static IEcsSystems InjectAspect(this IEcsSystems systems, params IEcsAspect[] injects) {
            if (injects == null) injects = System.Array.Empty<IEcsAspect>();

            foreach (var system in systems.GetAllSystems()) {
                InjectToSystem(system, injects);
            }

            foreach (var inject in injects) {
                InjectToAspect(inject, systems);
            }

            return systems;
        }

        private static void InjectToSystem(IEcsSystem system, IEcsAspect[] injects) {
            foreach (var f in system.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (f.IsStatic) continue;
                InjectCustoms(f, system, injects);
            }
        }

        private static void InjectCustoms(FieldInfo fieldInfo, IEcsSystem system, IEcsAspect[] injects) {
            if (typeof(IEcsCustomDataInject).IsAssignableFrom(fieldInfo.FieldType)) {
                var instance = (IEcsCustomDataInject)fieldInfo.GetValue(system);
                instance.Fill(injects);
                fieldInfo.SetValue(system, instance);
            }
        }

        private static void InjectToAspect(IEcsAspect inject, IEcsSystems systems) {
            foreach (var f in inject.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                if (f.IsStatic) continue;
                InjectBuiltIns(f, inject, systems);
            }
        }

        private static void InjectBuiltIns(FieldInfo fieldInfo, IEcsAspect inject, IEcsSystems systems) {
            if (typeof(IEcsDataInject).IsAssignableFrom(fieldInfo.FieldType)) {
                var instance = (IEcsDataInject)fieldInfo.GetValue(inject);
                instance.Fill(systems);
                fieldInfo.SetValue(inject, instance);
            }
        }
    }
}