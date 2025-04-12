using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class OutlineSettings
    {
        public Color outlineColor = Color.black;
        [Range(0.01f, 0.1f)] public float thickness = 0.02f;
        [Range(0, 1)] public float depthSensitivity = 0.5f;
    }

    public OutlineSettings settings = new OutlineSettings();
    private OutlinePass _outlinePass;
    private Material _outlineMaterial;

    public override void Create()
    {
        Shader outlineShader = Shader.Find("Shaders/OutlineShader");
        if (outlineShader == null)
        {
            Debug.LogError("Outline Shader not found. Ensure it's named 'Shaders/OutlineShader'");
            return;
        }

        _outlineMaterial = CoreUtils.CreateEngineMaterial(outlineShader);
        _outlinePass = new OutlinePass(_outlineMaterial, settings)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_outlineMaterial == null || settings.thickness <= 0)
            return;

        _outlineMaterial.SetColor("_OutlineColor", settings.outlineColor);
        _outlineMaterial.SetFloat("_OutlineThickness", settings.thickness);
        _outlineMaterial.SetFloat("_DepthSensitivity", settings.depthSensitivity);

        _outlinePass.Setup(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(_outlinePass);
    }

    private class OutlinePass : ScriptableRenderPass
    {
        private Material _material;
        private OutlineSettings _settings;
        private RTHandle _cameraColorTarget;
        private RTHandle _tempTexture;

        public OutlinePass(Material material, OutlineSettings settings)
        {
            _material = material;
            _settings = settings;
        }

        public void Setup(RTHandle colorTarget)
        {
            _cameraColorTarget = colorTarget;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref _tempTexture, desc, name: "_TempOutlineTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null || _cameraColorTarget == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Outline Pass");

            using (new ProfilingScope(cmd, new ProfilingSampler("OutlineEffect")))
            {
                Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _tempTexture, _material, 0);
                Blitter.BlitCameraTexture(cmd, _tempTexture, _cameraColorTarget);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            _tempTexture?.Release();
        }
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_outlineMaterial);
        _outlinePass?.OnCameraCleanup(null);
    }
}
