using UnityEditor;
using LightFramework.UI;
using UnityEngine;

namespace LightFrameworkEditor.UI
{
    [CustomEditor(typeof(UISelectableHandler), true)]
    class UISelectableHandlerEditor : Editor
    {
        SerializedProperty maxSelectNumField;
        SerializedProperty allowOffLastField;
        protected virtual void OnEnable()
        {
            maxSelectNumField = serializedObject.FindProperty("maxSelectNum");
            allowOffLastField = serializedObject.FindProperty("allowOffLast");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(maxSelectNumField, new GUIContent("最大可选数量"), true);
            EditorGUILayout.PropertyField(allowOffLastField, new GUIContent("允许取消最后选中项"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
