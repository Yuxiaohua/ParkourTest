using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIControl : MonoBehaviour
{
    private MainControl.CheckPoint checkPoint;
    public TMP_Text ScoreValueText;
    public TMP_Text HpText;
    public TMP_Text checkPointText;
    public static int score = 0;
    public static UIControl instance;
    void Start()
    {

        instance = this;
        checkPoint = MainControl.instance.getCurrentCheckPoint();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreValueText.text = $"Score: {score}/{checkPoint.goldCondition}";
        HpText.text = "Hp: " + PlayerControl.Hp;
        checkPointText.text = "Level: " + checkPoint.index;
    }

    public  void addScore(int addScore)
    {
        score += addScore;
    }

    public int getScore()
    {
        return score;
    }
}
