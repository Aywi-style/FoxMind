using FoxMind.Code.Runtime.ProjectScope;
using UnityEngine;

namespace FoxMind.Code.Runtime.Core.Ui
{
    public class CoreCanvas : MonoBehaviour
    {
        [SerializeField] public Canvas Canvas;
        [SerializeField] public RectTransform CanvasRectTransform;

        public void Awake()
        {
            R.CoreCanvas = this;
        }
    }
}