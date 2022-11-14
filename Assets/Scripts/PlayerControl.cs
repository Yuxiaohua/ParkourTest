using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public static int Hp = 3;
    public static bool pause = false;
    private Animator animator;
    private Rigidbody2D rigidbody;
    public bool IsGround = false;
    public static int score = 0;

    private static int left = 1;
    private static int right = 2;
    private int direction = right;
    //屏幕的一半
    private float CameraHalfWidth = 7.2f;
    private MainControl.CheckPoint checkPoint;
    private int maxJumpTimes = 2;
    private int jumpStep = 0;

    //无敌时间2秒，免死
    public static int UNSTOP_TIMES = 2;
    private bool isUnstop = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        checkPoint = MainControl.instance.getCurrentCheckPoint();
        Debug.Log("当前分数要求："+checkPoint.goldCondition);
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
        {
            return;
        }
        //按下空格
        if (Input.GetKeyDown(KeyCode.Space) && canJump())
        {
            jump();
        }
        
        float Horizontal = Input.GetAxis("Horizontal");

        //float Vertical = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(Mathf.Abs(Horizontal), 0);
        if (dir != Vector2.zero)
        {
            this.transform.Translate(dir * 5 * Time.deltaTime);
            Vector2 position = this.transform.position;
            if (position.x < -CameraHalfWidth)
            {
                position.x = -CameraHalfWidth;
            }
            else if(this.transform.position.x > CameraHalfWidth)
            {
                position.x = CameraHalfWidth;
            }
            this.transform.position = position;
            if (Horizontal > 0)
            {
                rightMove();
            }
            else
            {
                leftMove();
            }
        }
        else
        {
            rightMove();
        }

        if(UIControl.instance.getScore() >= checkPoint.goldCondition)
        {
            nextPoint();
        }

    }

    public void jump()
    {
        jumpStep += 1;
        rigidbody.AddForce(Vector2.up * 300);
        AduioManager.instance.play("jump");
    }

    public bool canJump()
    {
        if (jumpStep < maxJumpTimes)
        {
            return true;
        }
        return IsGround;
    }

    public void rightMove()
    {
        if(direction == right)
        {
            return;
        }
        direction = right;

        this.transform.Rotate(new Vector3(0, 1, 0), 180);

    }

    public void leftMove()
    {
        if (direction == left)
        {
            return;
        }
        direction = left;
        this.transform.Rotate(new Vector3(0, 1, 0), 180);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {

            jumpStep = 0;
            IsGround = true;
            animator.SetBool("isJump", false);
        }
        Debug.Log("OnCollisionEnter2D" + ":" + collision.collider.tag + " " + collision.collider.name);
        if (collision.collider.tag == "Die")
        {
            reduceHp();
            
        }

    }
    /**
     * 复活
     */
    private void revive()
    {
        animator.SetBool("isDie", false);
        this.animator.SetBool("revive", true);
        this.transform.position = new Vector3(-6.35f, 3f, 0);
        pause = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Debug.Log("OnCollisionExit" + ":" + collision.collider.tag);
            IsGround = false;
            animator.SetBool("isJump", true);
        }
        if(collision.collider.tag == "Enemy")
        {
            if (!isUnstop)
            {
                //非无敌状态，碰到敌人，就死亡
                Debug.Log("碰到Enemy");
                reduceHp();
            }
            
        }
    }


    private void nextPoint()
    {
        Debug.Log("分数达到要求，加载下一关1");
        MainControl.instance.saveToDisk();
        SceneManager.LoadScene(0);
        SceneManager.UnloadScene(1);
        Debug.Log("分数达到要求，加载下一关2");
    }

    private void gameOver()
    {
        SceneManager.LoadScene(2);
    }

    private void reduceHp()
    {
        if (pause)
        {
            return;
        }
        Debug.Log($"扣除血量:{Hp}");
        Hp -= 1;
        AduioManager.instance.play("Die");
        animator.SetBool("isDie", true);
        pause = true;
        if (Hp > 0)
        {
            Invoke("revive", 2f);
        }
        else
        {
            gameOver();
        }
        //进入无敌状态
        unstop();   
    }

    public void unstop()
    {
        isUnstop = true;
        Invoke("stopit", UNSTOP_TIMES);
    }

    public void stopit()
    {
        isUnstop = false;
    }

}
