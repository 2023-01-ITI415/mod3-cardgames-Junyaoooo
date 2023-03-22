using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextManager : MonoBehaviour
{

    [Header("Dynamic")]
    [SerializeField]
    private bool _resultUIFieldsVisible = false;

    public bool resultUIFieldsVisible 
    {
        get { return _resultUIFieldsVisible;}
        private set 
        {
            _resultUIFieldsVisible = value;
            gameOverText.gameObject.SetActive(value);
            roundResultText.gameObject.SetActive(value);
        }
    }


    [Header("Inscribed")]
    public TMP_Text gameOverText;
    public TMP_Text highScoreText;
    public TMP_Text roundResultText;

    private static UITextManager S;

    void ShowHighScore() 
    {
        string p = $"High Score: {ScoreManager.HIGH_SCORE:#,##0}";
        highScoreText.text = p;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (S != null) Debug.LogWarning("xxx");
        S = this;
        ShowHighScore();
        resultUIFieldsVisible = false;
    }

    static public void GAME_OVER_UI(bool won) 
    {
        S.GameOverUI(won);
    }

    public void GameOverUI(bool won) 
    {
        int score = ScoreManager.SCORE;
        string str;
        if (won)
        {
            gameOverText.text = "Round Over";
            str = "You won this round!\n"
                + $"Round Score: {ScoreManager.SCORE_THIS_ROUND:#,##0}";
        }
        else 
        {
            gameOverText.text = "Game Over";
            if (ScoreManager.HIGH_SCORE <= score)
            {
                str = $"You got the high score!\nHigh score:{score:#,##0}";
            }
            else 
            {
                str = $"Your final score was:\n{score:#,##0}";
            }
        }
        roundResultText.text = str;
        resultUIFieldsVisible = true;
        ShowHighScore();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
