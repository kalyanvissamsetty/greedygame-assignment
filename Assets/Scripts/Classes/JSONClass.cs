using System;

namespace GreedyGame.Class
{
    [Serializable]
    public class JSONClass
    {
        public string name;
        public Properties properties;
        public JSONClass[] newChildren;
    }

    [Serializable]
    public class Properties
    {
        public CustomVectors position;
        public CustomVectors rotation;
        public CustomVectors scale;
        public ColorData color;
        public UIAttribute uiAttribute;
    }
    
    [System.Serializable]
    public class ColorData
    {
        public float r, g, b, a;
    }

    [Serializable]
    public class CustomVectors
    {
        public float x;
        public float y;
        public float z;
    }
    
    [Serializable]
    public enum UIAttribute
    {
        GameObject,
        Button,
        Text,
        Image,
        RawImage
    }
}