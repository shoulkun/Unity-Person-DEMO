using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainCameraScriptableRenderer : ScriptableRenderer
{
    public MainCameraScriptableRenderer(ScriptableRendererData data) : base(data)
    {
    }

    public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // Calls AddRenderPasses for each renderer feature added to this renderer
        AddRenderPasses(ref renderingData);

        // Tell the pipeline the default render pipeline texture to use. When a scriptable render pass doesn't
        // set a render target using ConfigureTarget these render textures will be bound as color and depth by default.
        ConfigureCameraTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
    }

    public override void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
    {
    }

    public override void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
    {
    }
}
