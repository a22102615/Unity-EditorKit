using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Henry.EditorKit
{
    using Component;

    [Serializable]
    public class PinnedPanel : ISubPanel
    {
        StyleSheet style;

        // Initialize fields
        GUIStyle dotMenuBtnStyle;
        GUIContent cornerBtnGuiContent;
        GUILayoutOption emptyMinHeight;

        // Data fields
        [NonSerialized] List<Data> comps;
        Vector2 scrollPosition = Vector2.zero;

        Action requestLoadPresetComps;

        public event Action<Data> OnRequestUnpinComp;
        public event Action<Data> OnRequestPopupComp;
        public event Action OnRequestUnpinAllComp;

        public void Setup(Action requestLoadPresetComps)
        {
            this.requestLoadPresetComps = requestLoadPresetComps;
        }

        public void SetPinnedComps(List<Data> comps)
        {
            this.comps = comps;
        }

        void ISubPanel.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Unpin All Components"), false, UnpinAll);
            menu.AddSeparator(string.Empty);
        }

        void ISubPanel.OnEnable()
        {
            emptyMinHeight ??= GUILayout.MinHeight(0);

            foreach (var el in comps)
            {
                if (el.Component != null)
                {
                    el.Component.OnEnable();
                }
            }
        }

        void ISubPanel.OnGUI(Rect rect)
        {
            ValidateStyles();

            using (var view = new EditorGUILayout.ScrollViewScope(scrollPosition, false, false))
            {
                scrollPosition = view.scrollPosition;
                if (comps.Count == 0)
                {
                    DrawEmptyCompHint();
                }
                else
                {
                    DrawComponentCard(rect);
                }
            }

            void ValidateStyles()
            {
                if (style == null)
                {
                    style = StyleSheet.Instance;
                }
                if (dotMenuBtnStyle == null)
                {
                    dotMenuBtnStyle = new("SearchModeFilter") { stretchWidth = false };
                    dotMenuBtnStyle.fixedHeight = 20;
                    dotMenuBtnStyle.margin = new(0, 0, 2, 0);
                    dotMenuBtnStyle.padding = new(3, 3, 0, 0);
                }
                if (cornerBtnGuiContent == null)
                {
                    cornerBtnGuiContent = EditorGUIUtility.IconContent("_Menu");
                }
            }
        }

        void UnpinAll()
        {
            OnRequestUnpinAllComp?.Invoke();
        }

        void DrawEmptyCompHint()
        {
            EditorGUILayout.HelpBox("No pinned components\nYou can pin components from the [Browse] panel", MessageType.Info);
            using (new EditorGUILayout.VerticalScope(style.Block))
            {
                if (GUILayout.Button("Load preset components"))
                {
                    requestLoadPresetComps?.Invoke();
                }
            }
        }

        void DrawComponentCard(Rect rect)
        {
            var compsCache = comps;

            foreach (var el in compsCache)
            {
                var elPerferSize = el.Component.GetPreferMinSize();
                var minHeightOption = elPerferSize.y > 0 ? GUILayout.MinHeight(elPerferSize.y) : emptyMinHeight;
                using (new EditorGUILayout.VerticalScope(style.Block, minHeightOption))
                {
                    DrawCompHeader(el);
                    el.Component.OnGUI(rect);
                }
            }
        }

        void DrawCompHeader(Data data)
        {
            GUILayout.Space(2);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(data.Info.Config.Name, style.H1, style.Title_H1);

                if (GUILayout.Button(cornerBtnGuiContent, dotMenuBtnStyle))
                {
                    ShowContextMenu(data);
                }
            }
            GUILayout.Space(4);
        }

        void ShowContextMenu(Data data)
        {
            GenericMenu menu = new();
            menu.AddItem(new GUIContent("Unpin"), false, () => UnpinHandler(data));
            menu.AddItem(new GUIContent("Popup"), false, () => PopupHandler(data));
            menu.ShowAsContext();

            void UnpinHandler(Data data)
            {
                OnRequestUnpinComp?.Invoke(data);
            }

            void PopupHandler(Data data)
            {
                OnRequestPopupComp?.Invoke(data);
            }
        }
    }
}