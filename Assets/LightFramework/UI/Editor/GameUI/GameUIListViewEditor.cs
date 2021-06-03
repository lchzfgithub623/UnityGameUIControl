using UnityEditor;
using UnityEngine;

using LightFramework.UI;

namespace LightFrameworkEditor.UI
{
    [CustomEditor(typeof(GameUIListView), true)]
    public class GameUIListViewEditor : Editor
    {
        GameUIListView listView;

        SerializedProperty isReuseField;
        SerializedProperty paddingField;
        SerializedProperty spaceField;
        SerializedProperty isOptionalField;
        private bool isOptional;

        void OnEnable()
        {
            listView = target as GameUIListView;

            isReuseField = serializedObject.FindProperty("isReuse");
            paddingField = serializedObject.FindProperty("padding");
            spaceField = serializedObject.FindProperty("space");
            isOptionalField = serializedObject.FindProperty("isOptional");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            listView.ConsiderDirection = EditorGUILayout.Toggle("拖拽是否判断方向", listView.ConsiderDirection);

            EditorGUILayout.PropertyField(isReuseField, new GUIContent("可以复用列表项"));
            EditorGUILayout.PropertyField(spaceField, new GUIContent("列表项间距"), true);
            EditorGUILayout.PropertyField(paddingField, new GUIContent("上下左右边距"), true);
            EditorGUILayout.PropertyField(isOptionalField, new GUIContent("列表项是否可选"));

            serializedObject.ApplyModifiedProperties();

            if (isOptional != isOptionalField.boolValue)
            {
                isOptional = isOptionalField.boolValue;
                listView.RefreshSelectable();
            }
        }
    }
}

