using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckPointControl : MonoBehaviour
{
    public TMP_Text checkPointText;
    private MainControl.CheckPoint currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        currentPoint = MainControl.instance.getCurrentCheckPoint();
        Invoke("nextPoint", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        checkPointText.text = $"Level: {currentPoint.index + 1}";
    }

    public void nextPoint()
    {
        
        MainControl.instance.switchNextPoint();
        SceneManager.LoadScene(1);
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
    }

}
