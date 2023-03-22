using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum eScoreEvent
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
    public GameObject floatingScorePreFab;
    public float floatDuration = 0.76f;
    public Vector2 fsPosEnd = new Vector2(0.5f, 0.95f);
    public Vector2 fsPosMid = new Vector2(0.5f,0.90f);
    public Vector2 fsPosRun = new Vector2(0.5f,0.75f);
    public Vector2 fsPosMid2 = new Vector2(0.4f, 1.0f);

    


    [Tooltip("If true, then score events are logged to the Console")]
    public bool logScoreEvents = true;

    [Header("Check this box to reset the ProspectorHighScore to 100")]
    public bool checkToResetHighScore = false;

    [Header("Dynamic")]
    public int chain = 0;
    public int score = 0;
    public int scoreRun = 0;

    void FloatingScoreHandler(eScoreEvent evt) 
    {
        List<Vector2> fsPts;
        switch (evt) 
        {
            case eScoreEvent.mine:
                GameObject go = Instantiate<GameObject>(floatingScorePreFab);
                go.transform.SetParent(canvasTrans);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                FloatingScore fs=go.GetComponent<FloatingScore>();
                fs.score = chain;
                Vector2 mousePos = Input.mousePosition;
                mousePos.x /= Screen.width;
                mousePos.y/=Screen.height;

                fsPts = new List<Vector2>();
                fsPts.Add(mousePos);
                fsPts.Add(fsPosMid);
                fsPts.Add(fsPosRun);
                fs.fontSizes = new float[] { 10, 56, 10 };
                if (fsFirstInRun == null)
                {
                    fsFirstInRun = fs;
                    fs.fontSizes[2] = 48;

                }
                else 
                {
                    fs.FScallbackEvent += fsFirstInRun.FSCallBack;
                }
                fs.Init(fsPts, floatDuration);
                break;
            case eScoreEvent.draw:
            case eScoreEvent.gameWin:
            case eScoreEvent.gameLoss:
                if (fsFirstInRun != null) 
                {
                    fsPts=new List<Vector2>();
                    fsPts.Add(fsPosRun);
                    fsPts.Add(fsPosMid2);
                    fsPts.Add(fsPosEnd);
                    fsFirstInRun.fontSizes = new float[] { 48, 56, 10 };
                    fsFirstInRun.FScallbackEvent += ScoreBoard.FS_CALLBACK;
                    fsFirstInRun.Init(fsPts, floatDuration, 0);
                    fsFirstInRun = null;
                }
                break;
        }
    }

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
            PlayerPrefs.SetInt("ProspectorHighScore", 100);
            Debug.Log("PlayerPrefebs.ProspectorHighScore reset to 100!!!!!!!!!!");
        }
    }

    void Tally(eScoreEvent evt)
    {
        switch (evt)
        {
            case eScoreEvent.mine:
                chain++;
                scoreRun += chain;
                break;
            case eScoreEvent.draw:
            case eScoreEvent.gameWin:
            case eScoreEvent.gameLoss:
                chain = 0;
                score += scoreRun;
                scoreRun = 0;
                break;
        }
        string scoreStr = score.ToString("#,##0");
        switch (evt)
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
                if (HIGH_SCORE <= score)
                {
                    Log($"Game win. Your new high score was: {scoreStr}");
                    HIGH_SCORE = score;
                    PlayerPrefs.SetInt("ProspectorHighScore", score);
                }
                break;

            default:
                Log($"score:{scoreStr}, scoreRun:{scoreRun}, chain: {chain}");
                break;
        }
        FloatingScoreHandler(evt);
        if (evt == eScoreEvent.gameWin || evt == eScoreEvent.gameLoss) 
        {
            FloatingScore.REROUTE_TO_SCOREBOARD();
        }
    }

    void Log(string srr)
    {
        if (logScoreEvents) Debug.Log(srr);
    }

    static public int SCORE { get { return S.score; } }
    static public int SCORE_RUN { get { return S.scoreRun; } }
    static public int CHAIN { get { return S.chain; } }

    private FloatingScore fsFirstInRun;
    private Transform canvasTrans;

    void Start()
    {
        ScoreBoard.SCORE = SCORE;
        canvasTrans = GameObject.Find("Canvas").transform;
    }
}
