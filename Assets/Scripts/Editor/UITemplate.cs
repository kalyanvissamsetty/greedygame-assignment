using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GreedyGame.Class;
using Newtonsoft.Json;
using System.Text.Json;

namespace GreedyGame.EditorScripts
{
    public class UITemplate : EditorWindow
    {
        private TextAsset templateJson;
        private Vector2 scrollPosition;
        private GameObject parent;
        [MenuItem("GreedyGame/Template Instantiation")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow<UITemplate>("Template Instantiation");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Add a JSON File", EditorStyles.miniLabel);
            GUILayout.Space(4);
            templateJson = EditorGUILayout.ObjectField("JSON File", templateJson, typeof(TextAsset), false) as TextAsset;
            GUILayout.Space(20);
            GUILayout.Label("Add a GameObject reference to spawn UI", EditorStyles.miniLabel);
            GUILayout.Space(4);
            parent = EditorGUILayout.ObjectField("Parent GameObject", parent, typeof(GameObject), true) as GameObject;
            GUILayout.Space(20);
            if (GUILayout.Button("Generate UI in Hierarchy"))
            {
                if (templateJson != null)
                {
                    string json = templateJson.text;
                    InstantiateUITemplate(json);
                }
                else Debug.LogWarning("File Not found");
            }

            if (GUILayout.Button("Reset"))
            {
                templateJson = null;
                parent = null;
            }
        }

        private void InstantiateUITemplate(string json)
        {
            if (json.Length == 0)
            {
                Debug.LogWarning("Empty JSON file");
                return;
            }
            JSONClass templateData = JsonUtility.FromJson<JSONClass>(json);
            GameObject root = InstantiateUIElement(templateData);
            if (parent)
            {
                root.transform.SetParent(parent.transform);
            }
            //root.transform.SetParent(GameObject.Find("Generated UI").transform);
            Debug.Log("UI Created");
        }
        
        private GameObject InstantiateUIElement(JSONClass template)
        {
            GameObject uiElement = new GameObject(template.name);
            uiElement.transform.localPosition = new Vector3(template.properties.position.x, template.properties.position.y, template.properties.position.z);
            uiElement.transform.localRotation = Quaternion.Euler(template.properties.rotation.x, template.properties.rotation.y, template.properties.rotation.z);
            uiElement.transform.localScale = new Vector3(template.properties.scale.x, template.properties.scale.y, template.properties.scale.z);
            int attributeValue = (int)template.properties.uiAttribute;
            if (attributeValue == 1) // Button
            {
                uiElement.AddComponent<Button>();
            }
            else if (attributeValue == 2) // Text
            {
                uiElement.AddComponent<TMPro.TextMeshProUGUI>();
            }
            else if (attributeValue == 3) // Image
            {
                uiElement.AddComponent<Image>();
            }
            else if(attributeValue == 4)//Raw Image
            {
                uiElement.AddComponent<RawImage>();
            }

            foreach (var child in template.newChildren)
            {
                GameObject c = InstantiateUIElement(child);
                c.transform.SetParent(uiElement.transform);
            }
            return uiElement;
        }
    }
}
