using System.IO;
using GreedyGame.Class;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Controllers
{
    public class JSONExporterController : MonoBehaviour
    {
        [Header("JSON Extractor")]
        public GameObject selectGameObject;

        #region EditorFunctions
        public void ExportGameObjectToJson()
        {
            if (selectGameObject == null)
            {
                Debug.LogWarning("No game-object selected");
                return;
            }
            JSONClass hierarchyJson = CreateJsonHierarchy(selectGameObject.transform);
            string jsonString = JsonUtility.ToJson(hierarchyJson, true);
            string outputPath = "Assets/Resources/ExportedJSON/" + selectGameObject.transform.name + ".json";
            File.WriteAllText(outputPath, jsonString);
            Debug.Log("JSON File Exported to " + outputPath);
            ResetData();
        }
        
        public void ResetData()
        {
            selectGameObject = null;
        }
        #endregion

        #region MainFunction
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
            var childCount = transform.childCount;
            jsonObject.newChildren = new JSONClass[childCount];
            for (var i = 0; i < childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                jsonObject.newChildren[i] = CreateJsonHierarchy(childTransform);
            }
            return jsonObject;
        }
        #endregion
    }
}
