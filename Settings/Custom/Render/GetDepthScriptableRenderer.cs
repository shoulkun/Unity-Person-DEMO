using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

public class GetDepthScriptableRenderer : ScriptableRenderer
{
    private DepthOnlyPass m_DepthPrepass;
    private RenderTargetHandle m_DepthTexture;
    private Camera camera;
    private RenderTextureDescriptor cameraTargetDescriptor;
    private CameraData cameraData;
    public GetDepthScriptableRenderer(GetDepthScriptableRendererData data) : base(data)
    {
        m_DepthTexture.Init("_CameraDepthTexture");
        m_DepthPrepass = new DepthOnlyPass(RenderPassEvent.BeforeRenderingPrePasses, RenderQueueRange.opaque, data.opaqueLayerMask);
    }

    public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        cameraData = renderingData.cameraData;
        camera = cameraData.camera;
        cameraTargetDescriptor = cameraData.cameraTargetDescriptor;

        m_DepthPrepass.Setup(cameraTargetDescriptor, m_DepthTexture);
        EnqueuePass(m_DepthPrepass);

        for (int i = 0; i < rendererFeatures.Count; ++i)
        {
            if (rendererFeatures[i].isActive)
                rendererFeatures[i].AddRenderPasses(this, ref renderingData);
        }
        // AddRenderPasses(ref renderingData);
        ConfigureCameraTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
    }
}