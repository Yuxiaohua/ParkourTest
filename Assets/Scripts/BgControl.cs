using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgControl : MonoBehaviour
{
    public float speed = 0.2f;
    public float weith = 18.74f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("游戏场景已经加载");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControl.pause)
        {
            return;
        }
        foreach (Transform trans in transform)
        {
            Vector3 pos =  trans.position;
            pos.x -= speed * Time.deltaTime;

            if(pos.x <= -weith)
            {
                pos.x += weith * 2;
            }
            trans.position = pos;
        }

    }
}
