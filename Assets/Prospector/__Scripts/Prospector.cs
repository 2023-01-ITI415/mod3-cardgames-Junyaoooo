using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(JsonParseLayout))]
public class Prospector : MonoBehaviour
{
    private static Prospector S;

    [Header("Dynamic")]
    public List<CardProspector> drawPile;

    private Deck deck;
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
}
