using System;
using FoxMind.Code.Runtime.Core.Camera;
using FoxMind.Code.Runtime.Core.Stats.Features;
using FoxMind.Code.Runtime.ProjectScope;
using UnityEngine;
using UnityEngine.UI;

namespace FoxMind.Code.Runtime.Core.Battle.Targeting.Ui
{
    /// <summary>
    /// MonoBehaviour-представление текущей hard target цели на Canvas.
    /// Получает Transform точки цели и снимок статов через SetTarget, затем переводит world position в UI position.
    /// </summary>
    public class TargetingUi : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private RectTransform _targetingPoint;
        
        private Transform _target;
        private float _targetHealthPercent;

        public void Awake()
        {
            R.TargetingUI = this;
        }
        
        private void LateUpdate()
        {
            SetTargetingPointVisible();
            
            SetTargetingPointPosition();
        }

        private void SetTargetingPointVisible()
        {
            _targetingPoint.gameObject.SetActive(_target != null);
        }

        private void SetTargetingPointPosition()
        {
            if (_target == null)
            {
                return;
            }

            var screenPosition = R.CoreCamera.Camera.WorldToScreenPoint(_target.position);

            if (screenPosition.z < 0f)
            {
                _targetingPoint.gameObject.SetActive(false);
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                R.CoreCanvas.CanvasRectTransform,
                screenPosition,
                R.CoreCamera.Camera,
                out var localPoint
            );

            _targetingPoint.gameObject.SetActive(true);
            _targetingPoint.anchoredPosition = localPoint;
        }
        
        private void Update()
        {
            SetTargetHealth();
        }

        private void SetTargetHealth()
        {
            if (_target == null)
            {
                return;
            }
            
            _healthBar.fillAmount = _targetHealthPercent;
        }

        public static void SetTarget(Transform targetingPoint, UnitStatsComp statsComp)
        {
            if (R.TargetingUI == null)
            {
                return;
            }

            R.TargetingUI._target = targetingPoint;
            
            if (statsComp.EnergyMax != 0)
            {
                R.TargetingUI._targetHealthPercent = (float)statsComp.EnergyCurrent / statsComp.EnergyMax;
            }
            else
            {
                R.TargetingUI._targetHealthPercent = 0;
            }
        }
    }
}
