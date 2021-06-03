using UnityEditor;
using UnityEngine;
using LightFramework.UI;

namespace LightFrameworkEditor.UI
{
    [CustomEditor(typeof(GameUIToggle), true)]
    public class GameUIToggleEditor : Editor
    {
        SerializedProperty normalStateNodeField;
        SerializedProperty checkedStateNodeField;
        SerializedProperty labelField;
        SerializedProperty switchStateVisibleField;

        protected virtual void OnEnable()
        {
            normalStateNodeField = serializedObject.FindProperty("normalStateNode");
            checkedStateNodeField = serializedObject.FindProperty("checkedStateNode");
            labelField = serializedObject.FindProperty("label");
            switchStateVisibleField = serializedObject.FindProperty("switchStateVisible");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            serializedObject.Update();

            EditorGUILayout.PropertyField(normalStateNodeField, new GUIContent("正常状态节点"), true);
            EditorGUILayout.PropertyField(checkedStateNodeField, new GUIContent("选中状态节点"), true);
            EditorGUILayout.PropertyField(labelField, new GUIContent("标签"), true);
            EditorGUILayout.PropertyField(switchStateVisibleField, new GUIContent("显示或隐藏状态节点"), true);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

