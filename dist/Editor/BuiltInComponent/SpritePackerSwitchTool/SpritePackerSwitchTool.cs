using System;
using UnityEditor;
using UnityEngine;

namespace Henry.EditorKit.BuiltInComponent
{
    using Henry.EditorKit.Component;

    class SpritePackerSwitchTool : ScriptableObject, IComponent
    {
        readonly string[] optionsTitle = new string[] { "Disable", "V1", "V2", "Other" };

        int usingStateIndex;

        public static Config Info => new("SpritePacker Switcher")
        {
            Author = "林祐豪",
            Version = "1.0.0"
        };

        void IComponent.OnEnable()
        {
            usingStateIndex = GetCurrentPackerStateIndex();
        }

        void IComponent.OnDisable() { }

        void IComponent.OnGUI(Rect rect)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var tempSelectedStateIndex = GUILayout.Toolbar(usingStateIndex, optionsTitle);

                if (tempSelectedStateIndex != 3 && tempSelectedStateIndex != usingStateIndex)
                {
                    bool confirmed = EditorUtility.DisplayDialog(
                        "Confirm",
                        $"You are about to change the SpritePacker mode to [{optionsTitle[tempSelectedStateIndex]}]. Are you sure?",
                        "Confirm",
                        "Cancel"
                    );

                    if (confirmed)
                    {
                        usingStateIndex = tempSelectedStateIndex;
                        switch (usingStateIndex)
                        {
                            case 0: SetPackerMode(SpritePackerMode.Disabled); break;
                            case 1: SetPackerMode(SpritePackerMode.AlwaysOnAtlas); break;
                            case 2: SetPackerMode(SpritePackerMode.SpriteAtlasV2); break;
                        }
                    }
                }
            }

            usingStateIndex = GetCurrentPackerStateIndex();
        }

        int GetCurrentPackerStateIndex() => EditorSettings.spritePackerMode switch
        {
            SpritePackerMode.Disabled => 0,
            SpritePackerMode.AlwaysOnAtlas => 1,
            SpritePackerMode.SpriteAtlasV2 => 2,
            _ => 3
        };

        void SetPackerMode(SpritePackerMode mode)
        {
            EditorSettings.spritePackerMode = mode;
            AssetDatabase.SaveAssets();
        }
    }
}
