using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum eScoreEvent1
{
    draw,
    mine,
    gameWin,
    gameLoss,
}


public class ScoreManager : MonoBehaviour
{
    static private ScoreManager S;
    static public int HIGH_SCORE = 0;
    static public int SCORE_THIS_ROUND = 0;
    static public int SCORE_FROM_PREV_ROUND = 0;

    [Header("Inscribe")]
    [Tooltip("If true, then score events are logged to the Console")]
    public bool logScoreEvents = true;

    [Header("Check this box to reset the ProspectorHighScore to 100")]
    public bool checkToResetHighScore = false;

    [Header("Dynamic")]
    public int chain = 0;
    public int score = 0;
    public int scoreRun = 0;

    void Awake() 
    {
        if (S != null) 
        {
            Debug.LogError("ScoreManager.S is already set!");
        }

        S = this;
        if (PlayerPrefs.HasKey("ProspectorHighScore")) 
        {
            HIGH_SCORE = PlayerPrefs.GetInt("ProspectorHighScore");
        }

        score += SCORE_FROM_PREV_ROUND;
        SCORE_THIS_ROUND = 0;
    }

    static public void TALLY(eScoreEvent e) 
    {
        S.Tally(e);
    }

    void OnDrawGizmos()
    {
        if (checkToResetHighScore) 
        {
            checkToResetHighScore = false;
            PlayerPrefs.SetInt("ProspectorHighScore",100);
            Debug.Log("PlayerPrefebs.ProspectorHighScore reset to 100!!!!!!!!!!");
        }
    }

    void Tally(eScoreEvent ee) 
    {
        switch (ee) 
        {
            case eScoreEvent.mine:
                chain++;
                scoreRun += chain;
                break;
            case eScoreEvent.draw:
            case eScoreEvent.gameWin:
            case eScoreEvent.gameLoss:
                chain=0;
                score += scoreRun;
                scoreRun = 0;
                break;
       }
        string scoreStr=score.ToString("#,##0");
        switch (ee) 
        {
            case eScoreEvent.gameLoss:
                if (HIGH_SCORE <= score)
                {
                    Log($"game over, your new high score is: {scoreStr}");
                    HIGH_SCORE = score;
                    PlayerPrefs.SetInt("ProspectorHighScore", score);
                }
                else 
                {
                    Log($"game over. Your final score is: {scoreStr}");
                }
                SCORE_FROM_PREV_ROUND = 0;
                break;

            case eScoreEvent.gameWin:
                SCORE_THIS_ROUND = score - SCORE_FROM_PREV_ROUND;
                Log($"yOU WON THIS ROUND! ROUND SCORE IS: {SCORE_THIS_ROUND}");
                SCORE_FROM_PREV_ROUND = score;
                if (HIGH_SCORE<=score) 
                {
                    Log($"Game win. Your new high score was: {scoreStr}");
                    HIGH_SCORE= score;
                    PlayerPrefs.SetInt("ProspectorHighScore",score);
                }
                break;

            default:
                Log($"score:{scoreStr}, scoreRun:{scoreRun}, chain: {chain}");
                break;
        }
    }

    void Log(string srr)
    {
        if (logScoreEvents) Debug.Log(srr);
    }

    static public int SCORE { get { return S.score; } }
    static public int SCORE_RUN { get { return S.scoreRun; } }
    static public int CHAIN { get { return S.chain; } }
}
