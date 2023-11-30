using System;
using System.Collections;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace Fearness.Code.General
{
    public class SubsceneLoader : MonoBehaviour
    {
        [SerializeField] private SubScene _gameplaySubScene;
        [SerializeField] private bool _isLoadSubscene;

        private void Start()
        {
            LoadGameplaySubscene();
        }

        private void LoadGameplaySubscene()
        {
            if (!_isLoadSubscene)
            {
                return;
            }

            Debug.Log(_gameplaySubScene.SceneGUID);

            var loadParameters = new SceneSystem.LoadParameters() { Flags = SceneLoadFlags.NewInstance };
            var subSceneEntity = SceneSystem.LoadSceneAsync(World.DefaultGameObjectInjectionWorld.Unmanaged,
                _gameplaySubScene.SceneGUID, loadParameters);

            StartCoroutine(CheckIsLoaded(subSceneEntity));
        }

        private IEnumerator CheckIsLoaded(Entity entity)
        {
            bool isLoaded = false;

            do
            {
                Debug.Log(SceneSystem.GetSceneStreamingState(World.DefaultGameObjectInjectionWorld.Unmanaged, entity));
                isLoaded = SceneSystem.IsSceneLoaded(World.DefaultGameObjectInjectionWorld.Unmanaged, entity);

                yield return null;
                
            } while (!isLoaded);
            
            Debug.Log("Загружено");
        }
    }
}