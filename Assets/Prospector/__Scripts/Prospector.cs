using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(JsonParseLayout))]
public class Prospector : MonoBehaviour
{
    private static Prospector S;

    [Header("Dynamic")]
    public List<CardProspector> drawPile;
    public CardProspector target;
    public List<CardProspector> mine;
    public List<CardProspector> discardPile;



    private Deck deck;
    private Transform layoutAnchor;
    private JsonLayout jsonLayout;
    // Start is called before the first frame update
    void Start()
    {
        if (S != null) Debug.LogError("xx");
        S = this;
        jsonLayout=GetComponent<JsonParseLayout>().layout;
        deck=GetComponent<Deck>(); 
        deck.InitDeck();
        Deck.Shuffle(ref deck.cards);
        drawPile = ConvertCardsToCardProspectors(deck.cards);
        LayoutMine();
    }

    List<CardProspector> ConvertCardsToCardProspectors(List<Card> listCard) 
    {
        List<CardProspector> listCP = new List<CardProspector>();
        CardProspector cpp;
        foreach (Card card in listCard) 
        {
            cpp=card as CardProspector;
            listCP.Add(cpp);
        }
        return listCP;
    }

    void LayoutMine() 
    {
        if (layoutAnchor==null) 
        {
            GameObject tGO = new GameObject("_LayoutAnchor");
            layoutAnchor = tGO.transform;
        }

        CardProspector cp;
        foreach (JsonLayoutSlot s in jsonLayout.slots) 
        {
            cp = Draw();
            cp.faceUp= s.faceUp;
            cp.transform.SetParent(layoutAnchor);

            int z=int.Parse(s.layer[s.layer.Length-1].ToString());

            cp.SetlocalPos(new Vector3(
                jsonLayout.multiplier.x * s.x,
                jsonLayout.multiplier.y * s.y,
                -z));
            cp.layoutID = s.id;
            cp.layoutslot = s;
            cp.state = eCardState.mine;
            cp.SetSpriteSortingLayer(s.layer);
            mine.Add(cp);
        }
    }

    CardProspector Draw() 
    {
        CardProspector cp = drawPile[0];
        drawPile.RemoveAt(0); 
        return (cp);
    }
}
