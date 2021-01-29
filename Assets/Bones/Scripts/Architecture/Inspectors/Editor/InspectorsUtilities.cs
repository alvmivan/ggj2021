using System.Collections.Generic;
using UnityEngine;

namespace Bones.Scripts.Architecture.Inspectors.Editor
{
    public static class InspectorsUtilities
    {
        private static readonly Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

        private static readonly GUILayoutOption[] ButtonsSize =
        {
            GUILayout.Width(30),
            GUILayout.Height(30)
        };

        private static readonly GUILayoutOption[] ArrowSize =
        {
            GUILayout.Width(20),
            GUILayout.Height(20)
        };

        private static readonly GUILayoutOption[] PositionsButtonsSize =
        {
            GUILayout.Width(20),
            GUILayout.Height(20)
        };

        private static Texture GetTexture(string name)
        {
            if (!Textures.TryGetValue(name, out var tex))
            {
                tex = Resources.Load<Sprite>(name).texture;
                Textures[name] = tex;
            }

            return tex;
        }


        private static bool ImageButton(string imageName, GUIStyle guiStyle = null, params GUILayoutOption[] options)
        {
            var texture = GetTexture(imageName);
            if (guiStyle == null)
                return texture
                    ? GUILayout.Button(texture, options)
                    : GUILayout.Button(imageName, options);
            return texture
                ? GUILayout.Button(texture, guiStyle, options)
                : GUILayout.Button(imageName, guiStyle, options);
        }

        public static void ExpandButton(ref bool isExpanded)
        {
            if (!ImageButton(isExpanded ? "down" : "forward", GUIStyle.none, ArrowSize)) return;
            isExpanded = !isExpanded;
        }

        public static bool DeleteButton()
        {
            return ImageButton("cross", null, ButtonsSize);
        }

        public static bool AddButton()
        {
            return ImageButton("plus", null, ButtonsSize);
        }

        public static bool PositionFree()
        {
            return ImageButton("grey_box", GUIStyle.none, PositionsButtonsSize);
        }
    }
}