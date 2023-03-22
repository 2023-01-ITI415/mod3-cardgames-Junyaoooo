using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(BezierMover))]
public class FloatingScore : MonoBehaviour
{

    static List<FloatingScore> FS_ALL = new List<FloatingScore>();

    [Header("Dynamic")]
    [SerializeField]
    private int _score = 0;
    public int score
    {
        get { return (_score); }
        set
        {
            _score = value;
            textField.text = _score.ToString("#,##0");
        }
    }

    [Header("Inscribed")]
    public float[] fontSizes = { 10,56,48};

    public delegate void FloatingScoreDelegate(FloatingScore fs);
    public event FloatingScoreDelegate FScallbackEvent;

    private TMP_Text textField;
    private BezierMover mover;
    
    void Awake()
    {
        textField=GetComponent<TMP_Text>();
        mover=GetComponent<BezierMover>();
        //int numPoints = 4;
        //BezierMover mover=GetComponent<BezierMover>();
        //List<Vector2>points=new List<Vector2>();
        //Vector2 v2Half = new Vector2(0.5f, 0.5f);
        //points.Add(v2Half); ; 
        //for (int ss = 0; ss < numPoints; ss++) 
        //{
        //    points.Add(Random.insideUnitCircle*0.5f+v2Half);
        //}
        //mover.completionEvent.AddListener(MoverCompleteCallback);
        //mover.Init(points,numPoints);

    }

    void Update()
    {
        if (mover.state == BezierMover.eState.active) 
        {
            if (fontSizes!=null&&fontSizes.Length>0) 
            {
                float size = Utils.Bezier(mover.uCurved,fontSizes);
                textField.fontSize= size;
            }
        }
    }

    public void Init(List<Vector2> ePts, float eTimeD = 1, float eTimeS=0)
    {
        mover.completionEvent.AddListener(MoverCompleteCallback);
        mover.Init(ePts,eTimeD,eTimeS);
    }
   
    void MoverCompleteCallback()
    {
        if (FScallbackEvent != null) 
        {
            FScallbackEvent(this);
            FScallbackEvent = null;
            Destroy(gameObject);
        }
        //print("FloatingScore is done!");
    }

    void OnEnable()
    {
        FS_ALL.Add(this);
    }

    void OnDisable()
    {
        FS_ALL.Remove(this);
    }

    
    static public void REROUTE_TO_SCOREBOARD() 
    {
        Vector2 fsPosEnd = new Vector2(0.5f, 0.95f);
        foreach (FloatingScore fs in FS_ALL) 
        {
            fs.mover.bezierPts[fs.mover.bezierPts.Count-1]=fsPosEnd;
            fs.FScallbackEvent = null;
            fs.FScallbackEvent += ScoreBoard.FS_CALLBACK;
        }
        FS_ALL.Clear(); 
    }

    public void FSCallBack(FloatingScore fs) 
    {
        score += fs.score;
    }
}
