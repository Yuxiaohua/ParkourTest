using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverControl : MonoBehaviour
{
    public Text scoreText;
    private MainControl.CheckPoint checkPoint;
    // Start is called before the first frame update
    void Start()
    {
        checkPoint = MainControl.instance.getCurrentCheckPoint();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "关卡：" + checkPoint.index + "  分数：" + PlayerControl.score;
    }
}
