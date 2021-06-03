using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LightFrameworkEditor.UI
{

    internal static class GameUIMenuOptions
    {
        private const string kUILayerName = "UI";
        private const string TemplatePath = "UITemplatePrefab/";
        #region Create GameUI

        [MenuItem("GameObject/UI/GameUI/GameUIText", false, 2000)]
        static public void AddText(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIText");
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/GameUI/GameUIImage", false, 2001)]
        static public void AddImage(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIImage");
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/GameUI/GameUIRawImage", false, 2002)]
        static public void AddRawImage(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIRawImage");
            PlaceUIElementRoot(go, menuCommand);
        }

        // Controls

        // Button and toggle are controls you just click on.

        [MenuItem("GameObject/UI/GameUI/GameUIButton", false, 2030)]
        static public void AddButton(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIButton");
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/GameUI/GameUIToggle", false, 2031)]
        static public void AddToggle(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIToggle");
            PlaceUIElementRoot(go, menuCommand);
        }

        // Slider and Scrollbar modify a number

        [MenuItem("GameObject/UI/GameUI/GameUISlider", false, 2033)]
        static public void AddSlider(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUISlider");
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/GameUI/GameUIScrollbar", false, 2034)]
        static public void AddScrollbar(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIScrollbar");
            PlaceUIElementRoot(go, menuCommand);
        }

        // More advanced controls below

        [MenuItem("GameObject/UI/GameUI/GameUIDropdown", false, 2035)]
        static public void AddDropdown(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIDropdown");
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/GameUI/GameUIInputField", false, 2036)]
        public static void AddInputField(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIInputField");
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/GameUI/GameUIObject", false, 2060)]
        static public void AddGameUIObject(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIObject");
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/GameUI/GameUIPanel", false, 2061)]
        static public void AddPanel(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIPanel");
            PlaceUIElementRoot(go, menuCommand);

            // Panel is special, we need to ensure there's no padding after repositioning.
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
        }

        [MenuItem("GameObject/UI/GameUI/GameUIListView", false, 2062)]
        static public void AddScrollView(MenuCommand menuCommand)
        {
            GameObject go = GetGameUIResource("GameUIListView");
            PlaceUIElementRoot(go, menuCommand);
        }

        static GameObject GetGameUIResource(string name)
        {
            Object obj = Resources.Load(TemplatePath + name);
            if(obj == null)
            {
                Debug.LogError("请检查模版路径: " + (TemplatePath + name) + " 是否存在或者是否在'Resources/'路径下");
            }
            GameObject gObj = GameObject.Instantiate(obj) as GameObject;
            return gObj;
        }
        #endregion

        #region 从引擎源码中复制的 MenuOptions 内部辅助代码

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="element"></param>
        /// <param name="menuCommand"></param>
        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            if (parent == null || parent.GetComponentInParent<Canvas>() == null)
            {
                parent = GetOrCreateCanvasGameObject();
            }

            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
            element.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
            Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            GameObjectUtility.SetParentAndAlign(element, parent);
            if (parent != menuCommand.context) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

                Selection.activeGameObject = element;
        }

        private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null && SceneView.sceneViews.Count > 0)
                sceneView = SceneView.sceneViews[0] as SceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }

        /// <summary>
        /// Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
        /// </summary>
        /// <returns></returns>
        static public GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            // Try to find a gameobject that is the selected GO or one if its parents.
            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in selection or its parents? Then use just any canvas..
            canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (canvas != null && canvas.gameObject.activeInHierarchy)
                return canvas.gameObject;

            // No canvas in the scene at all? Then create a new one.
            return GameUIMenuOptions.CreateNewUI();
        }


        /// <summary>
        /// Helper methods
        /// </summary>
        /// <returns></returns>
        static public GameObject CreateNewUI()
        {
            // Root for the UI
            var root = new GameObject("Canvas");
            root.layer = LayerMask.NameToLayer(kUILayerName);
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.AddComponent<CanvasScaler>();
            root.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

            // if there is no event system add one...
            CreateEventSystem(false);
            return root;
        }

        private static void CreateEventSystem(bool select)
        {
            CreateEventSystem(select, null);
        }

        private static void CreateEventSystem(bool select, GameObject parent)
        {
            var esys = Object.FindObjectOfType<EventSystem>();
            if (esys == null)
            {
                var eventSystem = new GameObject("EventSystem");
                GameObjectUtility.SetParentAndAlign(eventSystem, parent);
                esys = eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
            }

            if (select && esys != null)
            {
                Selection.activeGameObject = esys.gameObject;
            }
        }

        #endregion
    }
}

