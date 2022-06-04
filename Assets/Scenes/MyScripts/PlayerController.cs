using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //角色刚体
    public new Rigidbody2D rigidbody2D;
    //初始移动速度
    public float speed;
    //跳跃力度
    public float jumpforce;
    //动画变量
    public Animator animator;
    //LayerMask 这个对象即要告诉系统哪个是真正的地面。这里拿到TileMap ，这里拿到的即是真实的ground。 
    public LayerMask ground;
    // 角色的碰撞体
    public Collider2D collider2D;
    // 樱桃得分
    public Text charryNumber;
    //如果受伤
    private bool isHurt; 

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (!isHurt)
        {
            MoveMent();
        }
        switchAnim();
    }

    //移动控制器。这里编写各种移动的方式
    void MoveMent()
    {
        float horizontalMove = Input.GetAxis("Horizontal");//如果返回值是1 则往右，如果返回值为-1 则往左
        float facedircetion = Input.GetAxisRaw("Horizontal");//这里可以直接获取到 -1 0 1 不会获取小数


        //角色移动
        if (horizontalMove != 0)//如果获取到移动大小不等于0的时候。肯定在移动
        {
            rigidbody2D.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rigidbody2D.velocity.y);
            //上述代码为移动，其中speed为速度。因为不存在跳跃，所以移动时不改变Y轴地址。

            animator.SetFloat("running", Mathf.Abs(horizontalMove));//给状态机设置状态值。
        }

        if (facedircetion != 0)//如果获取到到转向大小不等于0的时候，肯定在转向
        {
            transform.localScale = new Vector3(facedircetion, 1, 1);//因为Scale包括xyz三个上下左右方向的大小，所以这里需要使用三位向量
        }

        //角色跳跃
        if (Input.GetButtonDown("Jump") && collider2D.IsTouchingLayers(ground))
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpforce * Time.deltaTime);
            animator.SetBool("jumping", true);
        }
    }

    //切换动画
    void switchAnim()
    {
        animator.SetBool("idle", false);
        if (animator.GetBool("jumping"))//如果她是跳跃
        {
            //因为在跳跃时rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpforce * Time.deltaTime);
            //设置了一个jumpforce。即y轴跳跃的力度
            //所以一旦y轴的跳跃力 < 0   。。。即y轴的跳跃力没了，就代表他正在下降
            if (rigidbody2D.velocity.y < 0)//触发降落效果
            {
                animator.SetBool("jumping", false);
                animator.SetBool("failing", true);
            }
        }
        else if (isHurt)
        {
            if (Mathf.Abs(rigidbody2D.velocity.x) <0.1f)
            {
                isHurt = false;
            }
        }
        else if (collider2D.IsTouchingLayers(ground))
        {
            animator.SetBool("failing", false);
            animator.SetBool("idle", true);
        }
    }

    private int charry = 0;

    //收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collection")
        {
            charry += 1;
            Destroy(collision.gameObject);
            charryNumber.text = charry.ToString();
        }
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (animator.GetBool("failing"))
            {
                Destroy(collision.gameObject);
                //消灭敌人之后增加小跳
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpforce * Time.deltaTime * 0.5f);
                animator.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {//青蛙在右侧，往左侧移动
                rigidbody2D.velocity = new Vector2(-10, rigidbody2D.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x>collider2D.gameObject.transform.position.x)
            {
                rigidbody2D.velocity = new Vector2(10,rigidbody2D.velocity.y);
                isHurt = true;
            }
        }
    }

}