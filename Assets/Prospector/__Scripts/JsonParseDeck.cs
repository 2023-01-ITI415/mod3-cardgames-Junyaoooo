using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonPip 
{
    public float scale = 1;
    public bool flip = false;
    public Vector3 loc;
    public string type = "pip";
}

[System.Serializable]
public class JsonCard 
{
    public int rank;
    public List<JsonPip> pips=new List<JsonPip> ();
    public string face;

}


[System.Serializable]
public class JsonDeck 
{
    public List<JsonPip>decorators=new List<JsonPip> ();
    public List<JsonCard>cards=new List<JsonCard> ();   
}


public class JsonParseDeck : MonoBehaviour
{
    [Header("Inscribed")]
    public TextAsset jsonDeckFile;

    [Header("Dynamic")]
    public JsonDeck deck;


    private static JsonParseDeck S { get; set; }


    //// Start is called before the first frame update
    void Awake()
    {
        if (S != null)
        {
            Debug.LogError("Json be set a 2nd time!");
            return;
        }

        S = this;

        deck = JsonUtility.FromJson<JsonDeck>(jsonDeckFile.text);
    }


    static public List<JsonPip> DECORATORS
    {
        get { return S.deck.decorators; }
    }


    static public JsonCard GET_CARD_DEF(int UII)
    {
        if ((UII < 1) || (UII > S.deck.cards.Count))
        {
            Debug.LogWarning("ILLEGAL RAK ARGUMENTS: " + UII);
            return null;
        }
        return S.deck.cards[UII - 1];
    }
}
