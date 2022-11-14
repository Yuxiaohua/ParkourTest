using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandControl : MonoBehaviour
{
    public float speed = 2f;
    public float width = 11f;
    public float minLeft = -15f;
    
    public GameObject emptyGround;
    public GameObject enemyPerfeb;

    public GameObject[] coinPerfebs;
    public GameObject[] StoolPerfebs;
    private MainControl.CheckPoint checkPoint;
    // Start is called before the first frame update
    void Start()
    {
        checkPoint = MainControl.instance.getCurrentCheckPoint();
        speed = checkPoint.groundSpeed;
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
            Vector3 pos = trans.position;
            pos.x -= speed * Time.deltaTime;
            trans.position = pos;
            if (pos.x <= minLeft)
            {

                Transform lastTrans = this.getRightPos();
                GameObject newGround = this.newGround();
                //当前最后一个地块宽度
                float lastLandWidth = lastTrans.transform.Find("GroundLand").GetComponent<SpriteRenderer>().bounds.size.x;
                //新地块宽度
                float newLandWidth = newGround.transform.Find("GroundLand").GetComponent<SpriteRenderer>().bounds.size.x;
                Debug.Log("获取最右边的坐标:" + lastTrans.position.x);
                Debug.Log("获取地面宽度:" + lastLandWidth);

                Vector3 newPos = trans.position;
                float offset = Random.Range(2f, 5f);
                newPos.x = lastTrans.position.x + lastLandWidth/2 + newLandWidth/2 + offset;
                // y -2,2
                newPos.y = lastTrans.position.y + Random.Range(-4f, 2f);
                if (newPos.y > 2 || newPos.y < -4)
                {
                    newPos.y = 0;
                }
                Debug.Log("new position:" + newPos.x + "," + newPos.y);
                newGround.transform.position = newPos;
                Destroy(trans.gameObject);
            }

        }
    }

    private Transform getRightPos()
    {
        Transform lastTrans = transform.GetChild(0).transform;
        foreach (Transform trans in transform)
        {
            if (lastTrans.position.x < trans.position.x)
            {
                lastTrans = trans;
            }
        }
        return lastTrans;
    }

    private GameObject newGround()
    {
        float roundValue = Random.Range(0f, 1f);
        GameObject emptyObject = new GameObject();
        emptyObject.transform.parent = this.transform; 
        //随机地面宽度
        float scaleX = Random.Range(0.95f, 2f);
        GameObject newGround = Instantiate(emptyGround, emptyObject.transform);
        newGround.transform.position = new Vector3(0, 0);
        newGround.name = "GroundLand";
        // 横向拉伸
        //float scaleX = randWidth / newGround.GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("缩放：scaleX:"+ scaleX);
        newGround.transform.localScale = new Vector3(scaleX, 1, 1);
        float randWidth = newGround.GetComponent<SpriteRenderer>().bounds.size.x;
        float roundGoldValue = Random.Range(0f, 1f);
        //90%概率出现金币
        if (roundGoldValue <= 0.9f)
        {
            //随机一个金币
            GameObject newCoinObject = this.coinPerfebs[Random.Range(0, this.coinPerfebs.Length)];
            GameObject newCoin = Instantiate(newCoinObject, emptyObject.transform);
            newCoin.transform.localPosition = new Vector2(0, 2f);

        }
        float roundEnemyValue = Random.Range(0f, 1f);
        float maxValue = Mathf.Min(checkPoint.index * 0.1f,0.6f);
        //最大60%概率出现
        if (roundEnemyValue <= maxValue)
        {
            //随机一个敌人
            GameObject enemyObject = Instantiate(enemyPerfeb, emptyObject.transform);
            float x = (Random.Range(0f, randWidth) - randWidth / 2f) * 0.3f;
            enemyObject.transform.localPosition = new Vector2(x, 0.87f);
        }else if(roundEnemyValue <= 0.9f)
        {
            //随机一个凳子
            GameObject newStoolPerfebs = this.StoolPerfebs[Random.Range(0, this.StoolPerfebs.Length)];
            GameObject newStool = Instantiate(newStoolPerfebs, emptyObject.transform);
            float x = (Random.Range(0f, randWidth) - randWidth / 2f) * 0.5f;
            newStool.transform.localPosition = new Vector2(x, 0f);
        }
        return emptyObject;
    }
}
