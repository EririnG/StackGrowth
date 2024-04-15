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
        //���� ������ �� �ӵ� ����
        if (Input.GetButtonUp("Horizontal"))
        {
            //rigid.velocity.normalized.x�� �������͸� ���ϴ� ��
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //���� ��ȯ
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;//GetAxisRaw�� 1,0,-1�� ����

        ////���� ��, ���� �� animation ��ȯ
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
    private void FixedUpdate()//���� ���� �߻��ÿ��� ȣ��
    {
        float h = Input.GetAxisRaw("Horizontal");
        Debug.Log("h��"+h);

        rigid.AddForce(Vector2.right * h,ForceMode2D.Impulse);//�̶� ���ӷ��� ��� ������

        if (rigid.velocity.x > maxSpeed)//������ ����
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed * (-1)) //���� ����
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }

}
