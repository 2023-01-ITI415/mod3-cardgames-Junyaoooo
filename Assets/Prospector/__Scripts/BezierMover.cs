using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BezierMover : MonoBehaviour
{

    public enum eState 
    {
        idle,
        pre,
        post,
        active,
    }

    public enum ePosMode 
    {
        local,
        world,
        ugui,
    }

    [Header("Dynamic")]
    public eState state=eState.idle;
    public float timeStart = -1f;
    public float timeDuration = 1f;
    public List<Vector3> bezierPts;
    public string easingCurve = Easing.InOut;
    public UnityEvent completionEvent;

    public RectTransform rectTrans;
    public float uCurved { get; private set; }
    public float u { get; private set; }

    [Header("Inscribed")]
    [Tooltip("world sets transform.position"+
        "ugui sets anchorMin and anchorMax of RectTransform"+
        "local sets transform.localPosition")]
    public ePosMode posMode = ePosMode.ugui;

    public void Init(List<Vector2>ptsV2,float timeD=1,float timeS=0)
    {
        List<Vector3>ptsV3= new List<Vector3>();
        foreach (Vector2 v2 in ptsV2) { ptsV3.Add((Vector3)v2); }
        Init(ptsV3,timeD,timeS);
    }

    public void Init(List<Vector3> pts, float timeD = 1, float timeS = 0)
    {
        if (pts == null || pts.Count == 0) 
        {
            Debug.Log("You must pass at least one point into Init()!");
            return;
        }
        rectTrans= GetComponent<RectTransform>();
        pos = pts[0];

        if (pts.Count == 1) 
        {
            completionEvent.Invoke();
            return;
        }

        bezierPts=new List<Vector3>(pts);
        if (timeS == 0) timeS = Time.time;
        timeStart= timeS;
        timeDuration= timeD;
        state = eState.pre;
    }


    public Vector3 pos 
    {
        get 
        {
            if (posMode == ePosMode.ugui)
            {
                return rectTrans.anchorMin;
            }
            else if (posMode == ePosMode.local)
            {
                return transform.localPosition;
            }
            else 
            {
                return transform.position;
            }
        }
        private set 
        {
            if (posMode == ePosMode.ugui)
            {
                rectTrans.anchorMin=rectTrans.anchorMax=value;
            }
            else if (posMode == ePosMode.local)
            {
                transform.localPosition=value;
            }
            else
            {
               transform.position=value;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == eState.idle || state == eState.post) return;
        u = (Time.time - timeStart) / timeDuration;
        uCurved = Easing.Ease(u,easingCurve);
        if (u < 0)
        {
            state = eState.pre;
        }
        else 
        {
            if (u < 1)
            {
                state = eState.active;
            }
            else 
            {
                uCurved = 1;
                state = eState.post;
                completionEvent.Invoke();
            }
            pos = Utils.Bezier(uCurved,bezierPts);
        }
    }
}
