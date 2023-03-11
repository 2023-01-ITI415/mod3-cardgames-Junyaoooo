using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="CardSprites", menuName ="ScriptableObjects/CardSpritesSO")]



public class CardSpritesSO : ScriptableObject
{
    public Sprite suitClub;
    public Sprite suitDiamond;
    public Sprite suitHeart;
    public Sprite suitSpade;


    public Sprite[] faceSprites;
    public Sprite[] rankSprites;


    public Sprite cardBack;
    public Sprite cardBackGold;
    public Sprite cardFront;
    public Sprite cardFrontGold;

}
