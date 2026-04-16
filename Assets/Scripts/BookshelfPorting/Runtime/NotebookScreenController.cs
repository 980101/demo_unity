using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BookshelfPorting.Runtime
{
    public class NotebookScreenController : MonoBehaviour
    {
        private Canvas screenCanvas;
        private Text contentText;
        private readonly Button[] tabButtons = new Button[3];
        private string selectedTab = "Books";

        public Transform ScreenTransform { get; private set; }

        public void Configure(Transform screenTransform, Camera eventCamera, UnityAction onClose)
        {
            var shouldStartHidden = screenCanvas == null;
            ScreenTransform = screenTransform;
            EnsureCanvas(screenTransform, eventCamera, onClose);
            ApplyCanvasTransform();
            if (shouldStartHidden)
            {
                Hide();
            }
        }

        public void Show()
        {
            if (screenCanvas == null)
            {
                return;
            }

            screenCanvas.gameObject.SetActive(true);
            Refresh();
        }

        public void Hide()
        {
            if (screenCanvas != null)
            {
                screenCanvas.gameObject.SetActive(false);
            }
        }

        private void EnsureCanvas(Transform screenTransform, Camera eventCamera, UnityAction onClose)
        {
            if (screenCanvas != null || screenTransform == null)
            {
                return;
            }

            var canvasObject = new GameObject("NotebookScreenCanvas");
            canvasObject.transform.SetParent(screenTransform, false);

            screenCanvas = canvasObject.AddComponent<Canvas>();
            screenCanvas.renderMode = RenderMode.WorldSpace;
            screenCanvas.worldCamera = eventCamera;
            canvasObject.AddComponent<GraphicRaycaster>();

            var rect = screenCanvas.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(600f, 340f);

            var background = canvasObject.AddComponent<Image>();
            background.color = new Color(0.06f, 0.10f, 0.16f, 0.98f);

            CreatePanel(canvasObject.transform, "MenuBar", Vector2.zero, new Vector2(600f, 26f), new Vector2(0.5f, 1f), new Color(0.94f, 0.95f, 0.96f, 0.92f));
            CreateLabel(canvasObject.transform, "MenuTitle", "Notebook", new Vector2(16f, -4f), new Vector2(190f, 18f), 14, TextAnchor.UpperLeft, new Color(0.10f, 0.11f, 0.12f, 1f));
            CreateLabel(canvasObject.transform, "MenuStatus", "Search  Wi-Fi  10:09", new Vector2(-16f, -4f), new Vector2(158f, 18f), 13, TextAnchor.UpperRight, new Color(0.10f, 0.11f, 0.12f, 1f), new Vector2(1f, 1f));

            CreatePanel(canvasObject.transform, "AppWindowShadow", new Vector2(0f, -30f), new Vector2(586f, 312f), new Vector2(0.5f, 1f), new Color(0.18f, 0.21f, 0.26f, 0.34f));
            var appWindow = CreatePanel(canvasObject.transform, "AppWindow", new Vector2(0f, -28f), new Vector2(578f, 304f), new Vector2(0.5f, 1f), new Color(0.88f, 0.90f, 0.93f, 0.98f));
            CreatePanel(appWindow.transform, "WindowToolbar", new Vector2(0f, 132f), new Vector2(578f, 40f), new Vector2(0.5f, 0.5f), new Color(0.76f, 0.78f, 0.81f, 0.98f));
            var windowBody = CreatePanel(appWindow.transform, "WindowBody", new Vector2(0f, -20f), new Vector2(578f, 264f), new Vector2(0.5f, 0.5f), new Color(0.98f, 0.98f, 0.99f, 0.98f));
            CreateLabel(appWindow.transform, "WindowTitle", "Bookshelf Library", new Vector2(0f, 124f), new Vector2(220f, 22f), 15, TextAnchor.MiddleCenter, new Color(0.12f, 0.13f, 0.15f, 1f), new Vector2(0.5f, 0.5f));

            CreateButton(appWindow.transform, "CloseButton", "Close", new Vector2(-18f, -12f), onClose, new Vector2(78f, 24f), new Vector2(1f, 1f));

            tabButtons[0] = CreateButton(appWindow.transform, "BooksTab", "Books", new Vector2(-138f, 92f), () => SelectTab("Books"), new Vector2(122f, 30f), new Vector2(0.5f, 0.5f));
            tabButtons[1] = CreateButton(appWindow.transform, "UsersTab", "Users", new Vector2(0f, 92f), () => SelectTab("Users"), new Vector2(122f, 30f), new Vector2(0.5f, 0.5f));
            tabButtons[2] = CreateButton(appWindow.transform, "SettingsTab", "Settings", new Vector2(146f, 92f), () => SelectTab("Settings"), new Vector2(140f, 30f), new Vector2(0.5f, 0.5f));

            CreatePanel(windowBody.transform, "ContentPanel", new Vector2(0f, -10f), new Vector2(554f, 206f), new Vector2(0.5f, 0.5f), new Color(0.98f, 0.98f, 0.99f, 0f));
            contentText = CreateLabel(windowBody.transform, "ContentText", string.Empty, new Vector2(0f, -24f), new Vector2(506f, 172f), 16, TextAnchor.UpperLeft, new Color(0.16f, 0.17f, 0.18f, 1f), new Vector2(0.5f, 0.5f));

            Refresh();
        }

        private void ApplyCanvasTransform()
        {
            if (screenCanvas == null)
            {
                return;
            }

            var canvasTransform = screenCanvas.transform;
            canvasTransform.localPosition = new Vector3(0f, -0.022f, 0f);
            canvasTransform.localRotation = Quaternion.Euler(-90f, 0f, 180f);
            canvasTransform.localScale = Vector3.one * 0.00044f;
        }

        private void SelectTab(string tab)
        {
            selectedTab = tab;
            Refresh();
        }

        private void Refresh()
        {
            if (contentText == null)
            {
                return;
            }

            switch (selectedTab)
            {
                case "Users":
                    contentText.text = "Users\n- Search reader rooms\n- Visit profile shelves\n- Follow / Following placeholders";
                    break;
                case "Settings":
                    contentText.text = "Settings\n- Shelf privacy placeholder\n- Theme preference placeholder\n- Notification placeholder";
                    break;
                default:
                    contentText.text = "Books\n- Search books placeholder\n- Recent reviews placeholder\n- Saved reading list placeholder";
                    break;
            }

            UpdateTabButton(tabButtons[0], selectedTab == "Books");
            UpdateTabButton(tabButtons[1], selectedTab == "Users");
            UpdateTabButton(tabButtons[2], selectedTab == "Settings");
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchoredPosition, Vector2 size, Vector2 anchor)
        {
            return CreatePanel(parent, name, anchoredPosition, size, anchor, new Color(0.08f, 0.11f, 0.14f, 0.94f));
        }

        private static GameObject CreatePanel(Transform parent, string name, Vector2 anchoredPosition, Vector2 size, Vector2 anchor, Color color)
        {
            var panelObject = new GameObject(name);
            panelObject.transform.SetParent(parent, false);

            var image = panelObject.AddComponent<Image>();
            image.color = color;

            var rect = panelObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            return panelObject;
        }

        private static Button CreateButton(Transform parent, string name, string label, Vector2 anchoredPosition, UnityAction onClick, Vector2 size, Vector2 anchor)
        {
            var buttonObject = new GameObject(name);
            buttonObject.transform.SetParent(parent, false);

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.22f, 0.25f, 0.29f, 0.96f);

            var button = buttonObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(onClick);

            var rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;

            CreateLabel(buttonObject.transform, "Label", label, Vector2.zero, size, 16, TextAnchor.MiddleCenter);
            return button;
        }

        private static Text CreateLabel(Transform parent, string name, string textValue, Vector2 anchoredPosition, Vector2 size, int fontSize, TextAnchor alignment)
        {
            return CreateLabel(parent, name, textValue, anchoredPosition, size, fontSize, alignment, Color.white, alignment == TextAnchor.UpperLeft ? new Vector2(0f, 1f) : Vector2.zero);
        }

        private static Text CreateLabel(Transform parent, string name, string textValue, Vector2 anchoredPosition, Vector2 size, int fontSize, TextAnchor alignment, Color color)
        {
            return CreateLabel(parent, name, textValue, anchoredPosition, size, fontSize, alignment, color, alignment == TextAnchor.UpperLeft ? new Vector2(0f, 1f) : Vector2.zero);
        }

        private static Text CreateLabel(Transform parent, string name, string textValue, Vector2 anchoredPosition, Vector2 size, int fontSize, TextAnchor alignment, Color color, Vector2 anchor)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);

            var text = textObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.color = color;
            text.alignment = alignment;
            text.fontSize = fontSize;
            text.text = textValue;

            var rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = anchor;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
            return text;
        }

        private static void UpdateTabButton(Button button, bool isSelected)
        {
            if (button == null)
            {
                return;
            }

            button.GetComponent<Image>().color = isSelected
                ? new Color(0.12f, 0.39f, 0.84f, 0.96f)
                : new Color(0.31f, 0.34f, 0.38f, 0.96f);
        }

    }
}
