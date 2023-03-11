using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject prefabCard;
    public GameObject prefabSprite;
    public bool startFaceUp = true;
    public CardSpritesSO cardSprites;

    [Header("Dynamic")]
    public Transform deckAnchor;
    public List<Card> cards;

    private JsonParseDeck jsonDeck;

    static public GameObject SPRITE_PREFAB { get; private set; }


    //// Start is called before the first frame update
    //void Start()
    //{
    //    InitDeck();
    //   Shuffle(ref cards);
    //}


    Card MakeCard(char suit, int rank)
    {
        GameObject go = Instantiate<GameObject>(prefabCard, deckAnchor);
        Card card = go.GetComponent<Card>();
        card.Init(suit, rank, startFaceUp);
        return card;
    }


    void MakeCards()
    {
        cards = new List<Card>();
        Card c;

        string suits = "CDHS";
        for (int ii = 0; ii < 4; ii++)
        {
            for (int j = 1; j <= 13; j++)
            {
                c = MakeCard(suits[ii], j);
                cards.Add(c);
                c.transform.position = new Vector3((j - 7) * 3, (ii - 1.5f) * 4, 0);
            }
        }
    }


    public static void Shuffle(ref List<Card> refCards)
    {
        List<Card> tCards = new List<Card>();
        int ndx;
        while (refCards.Count > 0)
        {
            ndx = Random.Range(0, refCards.Count);
            tCards.Add(refCards[ndx]);
            refCards.RemoveAt(ndx);

        }
        refCards = tCards;
    }

    public void InitDeck()
    {
        SPRITE_PREFAB = prefabSprite;
        cardSprites.Init();
        jsonDeck = GetComponent<JsonParseDeck>();
        if (GameObject.Find("_Deck") == null)
        {
            GameObject anchorGo = new GameObject("_Deck");
            deckAnchor = anchorGo.transform;
        }

        MakeCards();
    }

}
