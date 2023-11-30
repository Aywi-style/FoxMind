using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fearness.Code.Runtime.Core.General
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
    }
}