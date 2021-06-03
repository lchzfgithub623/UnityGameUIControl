using UnityEditor;
using UnityEngine;
using LightFramework.UI;

namespace LightFrameworkEditor.UI
{
    /// <summary>
    /// 扩展 GameUIPanel 属性视图面板
    /// </summary>
    [CustomEditor(typeof(GameUIPanel), true)]
    public class GameUIPanelEditor : Editor
    {
        GameUIPanel panel;

        SerializedProperty bgColorField;

        protected virtual void OnEnable()
        {
            panel = target as GameUIPanel;
            bgColorField = serializedObject.FindProperty("bgColor");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            serializedObject.Update();

            panel.HasBgImage = EditorGUILayout.Toggle("是否有背景图:", panel.HasBgImage);
            EditorGUILayout.PropertyField(bgColorField, new GUIContent("背景非透明色值"), true);
            bool isTransparent = EditorGUILayout.Toggle("背景完全透明:", panel.IsTransparent);
            panel.IsTransparent = isTransparent;
            if (panel.IsTransparent != isTransparent)
            {
                EditorUtility.DisplayDialog("提示", "背景完全透明 不能生效, \n请检查选项: [是否有背景图] 是否勾选", "确定");
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

