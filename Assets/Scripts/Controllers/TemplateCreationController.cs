using System.IO;
using GreedyGame.Class;
using Newtonsoft.Json;
using UnityEngine;

namespace Controllers
{
    public class TemplateCreationController : MonoBehaviour
    {
        [Header("JSON Template Creator")]
        public JSONClass templateParent;

        #region EditorFunctions
        public void TemplateCreation()
        {
            string json = JsonConvert.SerializeObject(templateParent, Formatting.Indented);
            var templateName = templateParent.name.Equals("") ? "NoNamedJSON" : templateParent.name;
            string path = "Assets/Resources/" + templateName + ".json";
            File.WriteAllText(path, json);
            Debug.Log("Template created at " + path);
            ResetObject();
        }

        public void ResetObject()
        {
            templateParent = null;
        }
        #endregion
    }
}

