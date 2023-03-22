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
    public CardProspector target;
    public List<CardProspector> mine;
    public List<CardProspector> discardPile;

    [Header("Inscribed")]
    public float roundDelay = 2f;


    private Deck deck;
    private Transform layoutAnchor;
    private JsonLayout jsonLayout;

    private Dictionary<int, CardProspector> mineIdToCardDict;
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
        MoveToTarget(Draw());
        UpdateDrawPile();
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
        mineIdToCardDict = new Dictionary<int, CardProspector>();
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
            mineIdToCardDict.Add(s.id,cp);
        }
    }

    void MoveToDiscard(CardProspector cp) 
    {
        cp.state = eCardState.discard;
        discardPile.Add(cp);
        cp.transform.SetParent(layoutAnchor);
        cp.SetlocalPos(new Vector3(
            jsonLayout.multiplier.x * jsonLayout.discardPile.x,
            jsonLayout.multiplier.y* jsonLayout.discardPile.y,          
            0));
        cp.faceUp = true;
        cp.SetSpriteSortingLayer(jsonLayout.discardPile.layer);
        cp.SetSortingOrder(-200 + (discardPile.Count * 3));
    }

    void MoveToTarget(CardProspector cp) 
    {
        if (target != null) 
        {
            MoveToDiscard(target);
        }
        MoveToDiscard(cp);
        target = cp;
        cp.state = eCardState.target;
        cp.SetSpriteSortingLayer("Target");
        cp.SetSortingOrder(0);
    }

    void UpdateDrawPile() 
    {
        CardProspector cp;
        for (int h = 0; h < drawPile.Count; h++) 
        {
            cp = drawPile[h];
            cp.transform.SetParent(layoutAnchor);

            Vector3 cpPos = new Vector3();
            cpPos.x=jsonLayout.multiplier.x*jsonLayout.drawPile.x;
            cpPos.x += jsonLayout.drawPile.xStagger * h;
            cpPos.y = jsonLayout.multiplier.y*jsonLayout.drawPile.y;
            cpPos.z = 0.1f * h;
            cp.SetlocalPos(cpPos);
            cp.faceUp= false;
            cp.state= eCardState.drawpile;
            cp.SetSpriteSortingLayer(jsonLayout.drawPile.layer);
            cp.SetSortingOrder(-10 * h);
        }
    }

    public void SetMineFaceUps() 
    {
        CardProspector coverCP;
        foreach (CardProspector cp in mine) 
        {
            bool faceUp = true;
            foreach (int coverID in cp.layoutslot.hiddenBy) 
            {
                coverCP = mineIdToCardDict[coverID];
                if (coverCP == null || coverCP.state == eCardState.mine) 
                {
                    faceUp= false;
                }
            }
            cp.faceUp = faceUp;
        }
    }

    CardProspector Draw() 
    {
        CardProspector cp = drawPile[0];
        drawPile.RemoveAt(0); 
        return (cp);
    }

    void CheckGameOver() 
    {
        if (mine.Count == 0) 
        {
            GameOver(true);
            return;
        }
        if (drawPile.Count > 0) return;
        foreach (CardProspector cccc in mine) 
        {
            if (target.AdjacentTo(cccc)) 
            {
                return;
            }
        }
        GameOver(false);
    }

    static public void CARD_CLICKED(CardProspector cp) 
    {
        switch (cp.state) 
        {
            case eCardState.target:
                break;
            case eCardState.drawpile:
                S.MoveToTarget(S.Draw());
                S.UpdateDrawPile();
                ScoreManager.TALLY(eScoreEvent.draw);
                break;
            case eCardState.mine:
                bool validMatch = true;
                if(!cp.faceUp)validMatch= false;
                if(!cp.AdjacentTo(S.target)) validMatch= false;
                if (validMatch) 
                {
                    S.mine.Remove(cp);
                    S.MoveToTarget(cp);
                    S.SetMineFaceUps();
                    ScoreManager.TALLY(eScoreEvent.mine);
                }
                break;
        }

        S.CheckGameOver();
    }

    void ReloadLevel() 
    {
        SceneManager.LoadScene("_Prospector_Scene_0");
    }

    void GameOver(bool win1) 
    {
        if (win1)
        {
            ScoreManager.TALLY(eScoreEvent.gameWin);
        }
        else 
        {
            ScoreManager.TALLY(eScoreEvent.gameLoss);
        }

        CardSpritesSO.RESET();
        Invoke("ReloadLevel", roundDelay);
        UITextManager.GAME_OVER_UI(win1);
       // SceneManager.LoadScene("_Prospector_Scene_0");
    }
}
