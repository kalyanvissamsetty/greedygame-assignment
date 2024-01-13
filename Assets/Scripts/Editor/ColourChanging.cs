using System;
using UnityEngine;
using UnityEditor;

namespace GreedyGame.EditorScripts
{
    public class ExampleWindow : EditorWindow
    {
        private Color _color;
        [MenuItem("Window/Colorizer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ExampleWindow>("Colorizer");
        }
    
        private void OnGUI()
        {
            // Window Code
            GUILayout.Label("Color the selected Objects", EditorStyles.boldLabel);
            _color = EditorGUILayout.ColorField("Color", _color);
            if (GUILayout.Button("COLORIZE!"))
            {
                Colorize();
            }
        }

        private void Colorize()
        {
            foreach (var foreSelection in Selection.gameObjects)
            {
                Renderer renderer = foreSelection.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial.color = _color;
                }
            }
        }
    }

}
