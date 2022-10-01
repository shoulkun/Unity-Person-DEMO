using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class GetDepthScriptableRendererData : ScriptableRendererData
{

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Rendering/Get Depth Renderer", priority = CoreUtils.Priorities.editMenuPriority)]
    static void CreateForwardRendererData()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateCustomRendererAsset>(), "CustomRenderer.asset", null, null);
    }

    internal class CreateCustomRendererAsset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var instance = CreateInstance<GetDepthScriptableRendererData>();
            AssetDatabase.CreateAsset(instance, pathName);
            ResourceReloader.ReloadAllNullIn(instance, UniversalRenderPipelineAsset.packagePath);
            Selection.activeObject = instance;
        }
    }
#endif

    [SerializeField] LayerMask m_OpaqueLayerMask = -1;

    /// <summary>
    /// Use this to configure how to filter opaque objects.
    /// </summary>
    public LayerMask opaqueLayerMask
    {
        get => m_OpaqueLayerMask;
        set
        {
            SetDirty();
            m_OpaqueLayerMask = value;
        }
    }

    protected override ScriptableRenderer Create()
    {
        return new GetDepthScriptableRenderer(this);
    }
}