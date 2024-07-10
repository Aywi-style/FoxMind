using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoxMind.Code.Runtime.Core.General
{
    public class Bootstrap : MonoBehaviour
    {
        EcsWorld _world;
        IEcsSystems _systems;
        private void Start()
        {
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
        
        /*void Start () {
            // Создаем окружение, подключаем системы.
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            _systems
                .Add (new WeaponSystem ())
                .Init ();
        }
        
        void Update () {
            // Выполняем все подключенные системы.
            _systems?.Run ();
        }

        void OnDestroy () {
            // Уничтожаем подключенные системы.
            if (_systems != null) {
                _systems.Destroy ();
                _systems = null;
            }
            // Очищаем окружение.
            if (_world != null) {
                _world.Destroy ();
                _world = null;
            }
        }*/
    }
}