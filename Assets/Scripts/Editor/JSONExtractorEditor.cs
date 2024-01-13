using Controllers;
using UnityEngine;
using UnityEditor;
using GreedyGame.Controller;

namespace GreedyGame.EditorScripts
{
    [CustomEditor(typeof(JSONExporterController))]
    public class JSONExtractorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            JSONExporterController jsonExporterController = (JSONExporterController)target;
            if (GUILayout.Button("Generate JSON File"))
            {
                jsonExporterController.ExportGameObjectToJson();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Reset"))
            {
                jsonExporterController.ResetData();
            }
        }
    }

}
