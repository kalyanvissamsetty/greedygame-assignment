using Controllers;
using UnityEditor;
using UnityEngine;
using GreedyGame.Controller;

namespace GreedyGame.EditorScripts
{
    [CustomEditor(typeof(TemplateCreationController))]
    public class TemplateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TemplateCreationController templateCreationController = (TemplateCreationController)target;

            if (GUILayout.Button("Generate Template"))
            {
                templateCreationController.TemplateCreation();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Reset"))
            {
                templateCreationController.ResetObject();
            }
        }
    }
}

