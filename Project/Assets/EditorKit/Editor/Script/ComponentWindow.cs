using UnityEditor;
using UnityEngine;

namespace Henry.EditorKit
{
    using Henry.EditorKit.Component;

    sealed internal class ComponentWindow : EditorWindow
    {
        public static ComponentWindow Create(Data data)
        {
            ComponentWindow[] windows = Resources.FindObjectsOfTypeAll<ComponentWindow>();
            ComponentWindow window = System.Array.Find(windows, (w) => w);

            Rect? windowTargetPosition = null;
            if (window)
            {
                Rect position = window.position;
                position.position += new Vector2(25f, 25f);
                windowTargetPosition = position;
            }

            var compName = data.Info.Config.Name;
            window = CreateInstance<ComponentWindow>();
            window.titleContent = new GUIContent(compName);

            if (windowTargetPosition.HasValue)
            {
                window.shouldRepositionSelf = true;
                window.windowTargetPosition = windowTargetPosition.Value;
            }

            window.Setup(data);
            window.Show(true);
            window.Focus();

            return window;
        }

        [SerializeField] ScriptableObject componentSO;
        [SerializeField] string componentTypeFullName;
        [SerializeField] string contentStash;

        IComponent component;

        bool shouldRepositionSelf;
        Rect windowTargetPosition;

        bool isSetup = false;
        Vector2 scrollPosition = Vector2.zero;

        public void Setup(Data data)
        {
            componentSO = data.TargetSO;
            componentTypeFullName = data.Info.TypeFullName;
            contentStash = string.Empty;
            component = data.Component;

            isSetup = true;
        }

        void SetupFromLifeCycle()
        {
            if (string.IsNullOrEmpty(componentTypeFullName))
            {
                Close();
                return;
            }

            if (componentSO == null)
            {
                var componentInstance = InstanceStore.CreateComponent(componentTypeFullName);
                componentSO = componentInstance;
            }

            component = componentSO as IComponent;

            if (component == null)
            {
                Close();
                return;
            }
            else
            {
                component.OnInstance();
                component.OnEnable();
            }

            isSetup = true;
        }

        void OnDisable()
        {
            isSetup = false;
        }

        void OnDestroy()
        {
        }

        void Update()
        {
            if (shouldRepositionSelf)
            {
                shouldRepositionSelf = false;
                position = windowTargetPosition;
            }
        }

        void OnGUI()
        {
            if (isSetup is false)
            {
                SetupFromLifeCycle();
            }

            using (var view = new EditorGUILayout.ScrollViewScope(scrollPosition, false, false))
            {
                scrollPosition = view.scrollPosition;
                component.OnGUI(position);
            }
        }
    }
}