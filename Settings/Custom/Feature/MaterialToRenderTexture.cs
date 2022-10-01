using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MaterialToRenderTexture : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public Material blitMaterial = null;
        //public int blitMaterialPassIndex = -1;
        //目标RenderTexture 
        public RenderTexture renderTexture = null;

    }
    public Settings settings = new Settings();
    private CustomPass blitPass;


    public override void Create()
    {
        blitPass = new CustomPass(name, settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.blitMaterial == null)
        {
            //Debug.LogWarningFormat("丢失blit材质");
            return;
        }
        blitPass.renderPassEvent = settings.renderPassEvent;
        blitPass.Setup(renderer);
        renderer.EnqueuePass(blitPass);
    }
}

public class CustomPass : ScriptableRenderPass
{
    private MaterialToRenderTexture.Settings settings;
    string m_ProfilerTag;
    RenderTargetIdentifier source;
    CommandBuffer command;
    private ScriptableRenderer renderer;


    public CustomPass(string tag, MaterialToRenderTexture.Settings settings)
    {
        m_ProfilerTag = tag;
        this.settings = settings;
    }
                    //RenderTargetIdentifier src
    public void Setup(ScriptableRenderer renderer)
    {
        this.renderer = renderer;
        //source = renderer.cameraDepthTarget;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        source = renderer.cameraDepthTarget;
        command = CommandBufferPool.Get(m_ProfilerTag);
        command.Blit(source, settings.renderTexture, settings.blitMaterial);
        context.ExecuteCommandBuffer(command);
        CommandBufferPool.Release(command);
    }
}