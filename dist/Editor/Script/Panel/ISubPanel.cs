using UnityEditor;
using UnityEngine;

namespace Henry.EditorKit
{
    internal interface ISubPanel
    {
        void OnEnable() { }
        void OnDisable() { }
        void OnGUI(Rect rect);
        void AddItemsToMenu(GenericMenu menu) { }
    }
}