using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Dynamic")]
    public char suit;
    public Color color = Color.black;
    public string colS = "Black";
    public int rank;
    public GameObject back;
    public JsonCard def;

    public List<GameObject> pipGOs = new List<GameObject>();
    public List<GameObject> decoGOs = new List<GameObject>();

    public void Init(char eSuit, int eRank, bool startFaceUp = true)
    {
        gameObject.name = name = eSuit.ToString() + eRank;
        rank = eRank;
        suit = eSuit;

        if (suit == 'D' || suit == 'H')
        {
            colS = "Red";
            color = Color.red;
        }

        def = JsonParseDeck.GET_CARD_DEF(rank);
        AddDecorators();
        AddPips();
        AddFace();
        AddBack();
        faceUp = startFaceUp;

    }


    public virtual void SetlocalPos(Vector3 v)
    {

        transform.localPosition = v;
    }

    private Sprite _tSprite = null;
    private SpriteRenderer _tSRend = null;
    private GameObject _tGo = null;
    private Quaternion _flipRot = Quaternion.Euler(0, 0, 180);

    private void AddPips()
    {
        int pipNum = 0;
        foreach (JsonPip pip in def.pips)
        {
            _tGo = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
            _tGo.transform.localPosition = pip.loc;
            if (pip.flip) _tGo.transform.rotation = _flipRot;
            if (pip.scale != 1)
            {
                _tGo.transform.localScale = Vector3.one * pip.scale;
            }
            _tGo.name = "pip_" + pipNum++;

            _tSRend = _tGo.GetComponent<SpriteRenderer>();
            _tSRend.sprite = CardSpritesSO.SUITS[suit];
            _tSRend.sortingOrder = 1;
            pipGOs.Add(_tGo);
        }
    }

    private void AddBack()
    {
        _tGo = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
        _tSRend = _tGo.GetComponent<SpriteRenderer>();
        _tSRend.sprite = CardSpritesSO.BACK;
        _tGo.transform.localPosition = Vector3.zero;
        _tSRend.sortingOrder = 2;
        _tGo.name = "back";
        
        back = _tGo;
    }

    private void AddDecorators()
    {
        foreach (JsonPip pip in JsonParseDeck.DECORATORS)
        {
            if (pip.type == "suit")
            {
                _tGo = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
                _tSRend = _tGo.GetComponent<SpriteRenderer>();
                _tSRend.sprite = CardSpritesSO.SUITS[suit];
            }
            else
            {
                _tGo = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
                _tSRend = _tGo.GetComponent<SpriteRenderer>();
                _tSRend.sprite = CardSpritesSO.RANKS[rank];
                _tSRend.color = color;              
            }


            _tSRend.sortingOrder = 1;
            _tGo.transform.localPosition = pip.loc;
            if (pip.flip) _tGo.transform.rotation = _flipRot;
            if (pip.scale != 1)
            {
                _tGo.transform.localScale = Vector3.one * pip.scale;
            }

            _tGo.name = pip.type;
            decoGOs.Add(_tGo);
        }
    }

    private void AddFace()
    {
        if (def.face == "")
        {
            return;
        }
        string faceName = def.face + suit;
        _tSprite = CardSpritesSO.GET_FACE(faceName);
        if (_tSprite == null)
        {
            return;
        }
        _tGo = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
        _tSRend = _tGo.GetComponent<SpriteRenderer>();
        _tSRend.sprite = _tSprite;
        _tSRend.sortingOrder = 1;
        _tGo.transform.localPosition = Vector3.zero;
        _tGo.name = faceName;
    }

    public bool faceUp
    {
        get { return (!back.activeSelf); }
        set { back.SetActive(!value); }
    }

    private SpriteRenderer[] spriteRenderers;

    void PopulateSpriteRenderers() 
    {
        if (spriteRenderers != null) return;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetSpriteSortingLayer(string layerName) 
    {
        PopulateSpriteRenderers();
        foreach (SpriteRenderer srend in spriteRenderers) 
        {
            srend.sortingLayerName= layerName;
        }
    }

    public void SetSortingOrder(int oo) 
    {
        PopulateSpriteRenderers();
        foreach (SpriteRenderer srend in spriteRenderers) 
        {
            if (srend.gameObject == this.gameObject)
            {
                srend.sortingOrder = oo;
            }
            else if (srend.gameObject.name == "back")
            {
                srend.sortingOrder = oo + 2;
            }
            else 
            {
                srend.sortingOrder = oo+1;
            }
        }
    }



}
