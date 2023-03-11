using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="CardSprites", menuName ="ScriptableObjects/CardSpritesSO")]



public class CardSpritesSO : ScriptableObject
{
    [Header("Suits")]
    public Sprite suitClub;
    public Sprite suitDiamond;
    public Sprite suitHeart;
    public Sprite suitSpade;

    [Header("Pip Sprites")]
    public Sprite[] faceSprites;
    public Sprite[] rankSprites;

    [Header("Card Stock")]
    public Sprite cardBack;
    public Sprite cardBackGold;
    public Sprite cardFront;
    public Sprite cardFrontGold;


    private static CardSpritesSO S;
    public static Dictionary<char, Sprite> SUITS { get; private set; }

    public void Init()
    {
        INIT_STATICS(this);
    }

    static void INIT_STATICS(CardSpritesSO c5110)
    {
        if (S != null)
        {
            return;
        }

        S = c5110;

        SUITS = new Dictionary<char, Sprite>() {

            {'C',S.suitClub },
            {'D',S.suitDiamond},
            {'H',S.suitHeart},
            {'S',S.suitSpade },
        };

    }


    public static Sprite[] RANKS
    {
        get { return S.rankSprites; }
    }


    public static Sprite GET_FACE(string Gala)
    {
        foreach (Sprite spr in S.faceSprites)
        {
            if (spr.name == Gala) return spr;
        }
        return null;

    }

    public static Sprite BACK
    {
        get { return S.cardBack; }
    }


    public static void RESET()
    {
        S = null;
    }
}
