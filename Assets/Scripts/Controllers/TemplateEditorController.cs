using System.Collections;
using System.Collections.Generic;
using System.IO;
using GreedyGame.Class;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GreedyGame.Controller
{
    public class TemplateEditorController : MonoBehaviour
    {
        public GameObject selectGameObject;
        public JSONClass templateLoaded;
        private string jsonString;
        private bool isLoadClicked;
        
        #region Editor Functions
        public void ExportGameObjectToJson()
        {
            if (selectGameObject == null)
            {
                Debug.LogWarning("No game-object selected");
                return;
            }
            isLoadClicked = true;
            JSONClass hierarchyJSON = CreateJsonHierarchy(selectGameObject.transform);
            jsonString = JsonUtility.ToJson(hierarchyJSON, true);
            LoadTemplateToInspector(jsonString);
        }
        
        public void SaveUpdatedData() // Saving the data to json
        {
            if (!isLoadClicked)
            {
                isLoadClicked = false;
                Debug.LogWarning("Load Template not clicked");
                return;
            }
            if (selectGameObject == null)
            {
                Debug.LogWarning("No game-object selected");
                return;
            }
            string updatedJson = JsonConvert.SerializeObject(templateLoaded, Formatting.Indented);
            if (updatedJson.Equals(jsonString)) Debug.LogError("noooo   ");
            templateLoaded = JsonUtility.FromJson<JSONClass>(updatedJson);
            Debug.Log("Template Updated");
            string path = "Assets/Resources/" + templateLoaded.name + ".json";
            File.WriteAllText(path, updatedJson);
            Debug.Log("JSON File Updated");
            UpdateGameObject();
        }
        
        public void ResetData()
        {
            jsonString = "";
            templateLoaded = null;
            selectGameObject = null;
            isLoadClicked = false;
        }
        #endregion

        private bool CheckingJson(string jsonString1, string jsonString2)
        {
            Dictionary<string, object> dict1 = JsonUtility.FromJson<Dictionary<string, object>>(jsonString1);
            Dictionary<string, object> dict2 = JsonUtility.FromJson<Dictionary<string, object>>(jsonString2);

            // Convert dictionaries to JSON strings for comparison (optional)
            string reconstructedJson1 = JsonUtility.ToJson(dict1);
            string reconstructedJson2 = JsonUtility.ToJson(dict2);

            // Compare JSON strings or compare dictionaries directly
            return reconstructedJson1 == reconstructedJson2;
        }
        

        private void LoadTemplateToInspector(string jsonString) // Loading of data from json to inspector
        {
            JSONClass templateData = JsonUtility.FromJson<JSONClass>(jsonString);
            templateLoaded = templateData;
            Debug.Log("Templated Loaded");
        }

        
        
        private JSONClass CreateJsonHierarchy(Transform transform)
        {
            var position = transform.position;
            var eulerAngles = transform.eulerAngles;
            var localScale = transform.localScale;
            var ans = UIAttribute.GameObject;
            
            if (transform.GetComponent<Button>() != null)
            {
                ans = UIAttribute.Button;
            }
            else if (transform.GetComponent<TextMeshProUGUI>() != null)
            {
                ans = UIAttribute.Text;
            }
            else if (transform.GetComponent<Image>() != null)
            {
                ans = UIAttribute.Image;
            }
            else if (transform.GetComponent<RawImage>() != null)
            {
                ans = UIAttribute.RawImage;
            }

            JSONClass jsonObject = new JSONClass
            {
                name = transform.name,
                properties = new Properties()
                {
                    position = new CustomVectors
                        { x = position.x, y = position.y, z = position.z },
                    rotation = new CustomVectors
                        { x = eulerAngles.x, y = eulerAngles.y, z = eulerAngles.z },
                    scale = new CustomVectors
                        { x = localScale.x, y = localScale.y, z = localScale.z },
                    color = new ColorData { r = 0, g = 0, b = 0, a = 1 },
                    
                    uiAttribute = ans
                }
            };
            int childCount = transform.childCount;
            jsonObject.newChildren = new JSONClass[childCount];
            for (int i = 0; i < childCount; i++)
            {
                var childTransform = transform.GetChild(i);
                jsonObject.newChildren[i] = CreateJsonHierarchy(childTransform);
            }
            return jsonObject;
        }

        private void UpdateGameObject()
        {
            JSONClass templateData = templateLoaded;
            GameObject root = CreateUIELements(templateData);
            root.transform.SetParent(GameObject.Find("Generated UI").transform);
            Debug.Log("Updated GameObject");
            DestroyImmediate(selectGameObject);
            ResetData();
        }

        private GameObject CreateUIELements(JSONClass template)
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
            else if(attributeValue == 4)
            {
                uiElement.AddComponent<RawImage>();
            }
            foreach (var child in template.newChildren)
            {
                GameObject c = CreateUIELements(child);
                c.transform.SetParent(uiElement.transform);
            }
            return uiElement;
        }
    }
}
