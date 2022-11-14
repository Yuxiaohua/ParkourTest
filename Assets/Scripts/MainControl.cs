using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MainControl : MonoBehaviour
{

    private static int index = 0;
    public float baseSpeed = 2f;
    public float diffSpeed = 0.5f;
    public int baseGold = 100;
    public static MainControl instance;
    // Start is called before the first frame update
    private void Awake()
    {
        initGame();
    }

    void Start()
    {
        instance = this;
        Debug.Log("Instance MainControl");
    }

    /**
     * 1、支持获取金币显示
    2、支持默认3条命
    3、支持反向运动
    4、支持地面难度随机，随机因素：敌人数量、地面高度
    5、支持随机金币类型，单个、V型、^型和一字型。
    6、支持关卡，关卡过关一金币为过关条件。关卡难度因素：金币（2>>当前关卡 * 100）、速度（基础速度 * 当前关卡）。 
     */
    public CheckPoint GetCheckPoint(int index)
    {
        CheckPoint checkPoint = new CheckPoint();
        checkPoint.goldCondition = (2 << index) * baseGold;
        checkPoint.groundSpeed = baseSpeed + diffSpeed * index;
        checkPoint.index = index;
        return checkPoint;
    }
    //获取下一个关卡
    public CheckPoint getNextCheckPoint()
    {
        return GetCheckPoint(index + 1);
    }

    public CheckPoint getCurrentCheckPoint()
    {
        return GetCheckPoint(index);
    }

    public CheckPoint switchNextPoint()
    {
        index += 1;
        CheckPoint checkPoint = getCurrentCheckPoint();
        Debug.Log("加载下一关:"+ checkPoint.index);
        return checkPoint;
    }

    
    public struct CheckPoint
    {
        public int index;
        public float goldCondition;
        public float groundSpeed;
    }

    public void initGame()
    {
        UserData  userData = fromDisk();
        if(userData == null)
        {
            return;
        }
        Debug.Log($"游戏初始化：level:{userData.level},score: {userData.score},hp: {userData.hp}");
        UIControl.score = userData.score;
        PlayerControl.Hp = userData.hp;
        MainControl.index = userData.level;
    }

    public UserData fromDisk()
    {
        try
        {
            string filePath = this.getJsonFilePath();
            string jsonTest = File.ReadAllText(filePath, Encoding.UTF8);
            Debug.Log("获取到用户数据：" + jsonTest);
            UserData userData = JsonUtility.FromJson<UserData>(jsonTest);

            return userData;
        }catch
        {
            return null;
        }
    }

    public void saveToDisk()
    {
        UserData userData = new UserData();
        userData.score = UIControl.score;
        userData.level = this.getCurrentCheckPoint().index;
        userData.hp = PlayerControl.Hp;
        string data = JsonUtility.ToJson(userData);
        Debug.Log("存储用户数据："+ data);
        File.WriteAllText(getJsonFilePath(), data);
    }

    private string getJsonFilePath()
    {
        return Application.dataPath + "/Resources/parkour.json";
    }
}
