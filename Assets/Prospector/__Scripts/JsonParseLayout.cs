using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JsonLayout 
{
    public Vector2 multiplier;
    public JsonLayoutPile drawPile, discardPile;
    public List<JsonLayoutSlot> slots;
}

[System.Serializable]
public class JsonLayoutSlot : ISerializationCallbackReceiver 
{
    public int id;
    public int x;
    public int y;
    public string layer;
    public string hiddenByString;
    public bool faceUp;

    [System.NonSerialized]
    public List<int> hiddenBy;

    public void OnAfterDeserialize() 
    {
        hiddenBy= new List<int>();
        if (hiddenByString.Length==0)  
        {
            return;
        }

        string[] bits=hiddenByString.Split(',');
        for (int k = 0; k < bits.Length; k++) 
        {
            hiddenBy.Add(int.Parse(bits[k]));
        }
    }


    public void OnBeforeSerialize() { }


}

[System.Serializable]
public class JsonLayoutPile
{
    public int x, y;
    public float xStagger;
    public string layer;
}


public class JsonParseLayout : MonoBehaviour
{
    public static JsonParseLayout S { get; private set; }

    [Header("Inscribed")]
    public TextAsset jsonLayoutFile;

    [Header("Dynamic")]
    public JsonLayout layout;

    private void Awake()
    {
        layout = JsonUtility.FromJson<JsonLayout>(jsonLayoutFile.text);
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
