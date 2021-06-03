using UnityEditor;
using UnityEngine;

using LightFramework.UI;

namespace LightFrameworkEditor.UI
{

    [CustomEditor(typeof(GameUIMono), true)]
    public class GameUIMonoEditor:Editor
    {
        GameUIMono uiMono;

        protected void OnEnable()
        {
            uiMono = target as GameUIMono;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("UI Class Type:");
            EditorGUILayout.TextField((uiMono != null && uiMono.UIBase != null) ? uiMono.UIBase.GetType().ToString() : "");
        }
    }
}
