using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    public float maxSpeed;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //손을 떼었을 때 속도 조절
        if (Input.GetButtonUp("Horizontal"))
        {
            //rigid.velocity.normalized.x는 단위벡터를 구하는 것
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //방향 전환
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;//GetAxisRaw는 1,0,-1만 가능

        ////걸을 때, 멈출 때 animation 변환
        //Debug.Log(Mathf.Abs(rigid.velocity.x));

        //if (Mathf.Abs(rigid.velocity.x) < 0.3)
        //{            
        //    anim.SetBool("isWalking", false);
        //}
        //else
        //{
        //    anim.SetBool("isWalking", true);
        //}

    }
    private void FixedUpdate()//물리 연산 발생시에만 호출
    {
        float h = Input.GetAxisRaw("Horizontal");
        Debug.Log("h값"+h);

        rigid.AddForce(Vector2.right * h,ForceMode2D.Impulse);//이때 가속력이 계속 가해짐

        if (rigid.velocity.x > maxSpeed)//오른쪽 방향
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed * (-1)) //왼쪽 방향
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }

}
