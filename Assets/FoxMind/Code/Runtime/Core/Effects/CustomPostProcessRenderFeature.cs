using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Effects
{
    public class CustomPostProcessRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private Shader m_bloomShader;
        [SerializeField] private Shader m_compositeShader;

        private Material _bloomMaterial;
        private Material _compositeMaterial;
        
        private CustomPostProcessPass m_customPass;
        
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.Game)
            {
                renderer.EnqueuePass(m_customPass);
            }
        }

        public override void Create()
        {
            _bloomMaterial = CoreUtils.CreateEngineMaterial(m_bloomShader);
            _compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);
            
            m_customPass = new CustomPostProcessPass(_bloomMaterial, _compositeMaterial);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.Game)
            {
                m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);
                m_customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
            }
            //base.SetupRenderPasses(renderer, in renderingData);
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(_bloomMaterial);
            CoreUtils.Destroy(_compositeMaterial);
            //base.Dispose(disposing);
        }
    }
}