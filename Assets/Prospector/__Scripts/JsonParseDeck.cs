using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonPip 
{
    public float scale = 1;
    public bool flip = false;
    public Vector3 loc;
    public string type = "pip";
}


public class JsonCard 
{
    public int rank;
    public List<JsonPip> pips=new List<JsonPip> ();
    public string face;

}



public class JsonDeck 
{
    public List<JsonPip>decorators=new List<JsonPip> ();
    public List<JsonCard>cards=new List<JsonCard> ();   
}


public class JsonParseDeck : MonoBehaviour
{
    public TextAsset jsonDeckFile;
    public JsonDeck deck;
    // Start is called before the first frame update
    void Awake()
    {
        deck = JsonUtility.FromJson<JsonDeck>(jsonDeckFile.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
