using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.Rendering.Universal;

[CustomEditor(typeof(MainCameraScriptableRendererData), true)]
public class MainCameraScriptableRendererDataEditor : ScriptableRendererDataEditor
{
    SerializedProperty m_OpaqueLayerMask;
    SerializedProperty m_TransparentLayerMask;

    private void OnEnable()
    {
        m_OpaqueLayerMask = serializedObject.FindProperty("m_OpaqueLayerMask");
        m_TransparentLayerMask = serializedObject.FindProperty("m_TransparentLayerMask");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_OpaqueLayerMask);
        EditorGUILayout.PropertyField(m_TransparentLayerMask);

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}