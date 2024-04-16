using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    Animator anim; // Animation을 control 하기 위해서
    Rigidbody2D rigid;
    Vector3 dirVec;
    GameObject scanObject;

    public GameObject Player; 
    public GameObject uiPost; //public으로 선언해주면 unity에서 드래그앤 드랍 가능


    public float Speed;
    float x;
    float y;
    bool isHorizonMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        //누르고 있으면 0 ,-1 ,1
        bool xDown = Input.GetButtonDown("Horizontal");
        bool yDown = Input.GetButtonDown("Vertical");
        bool xUp = Input.GetButtonUp("Horizontal");
        bool yUp = Input.GetButtonUp("Vertical");

        //수평이동인지 확인
        if (xDown)
        {
            isHorizonMove = true;
        }
        else if (yDown)
        {
            isHorizonMove = false;
        }
        else if (xUp || yUp)//수평,수직을 동시에 눌렀다 떼었을 때를 말한다.
        {
            isHorizonMove = x != 0;
        }
        
        //if문이 없다면 계속 다시 실행시킨다.-> 즉 , 애니메이션이 처음 프레임에서 멈춘 상태로 끝난다.
        if(anim.GetInteger("xAxisRaw") != x)
            {
                anim.SetBool("isChange", true);
                anim.SetInteger("xAxisRaw", (int)x);
            }
        else if(anim.GetInteger("yAxisRaw") != y)
            {
            anim.SetBool("isChange", true);
            anim.SetInteger("yAxisRaw", (int)y);
            }
        else
        {
            anim.SetBool("isChange", false);
        }


        if (yDown && y == 1)
            dirVec = Vector3.up;
        else if (yDown && y == -1)
            dirVec = Vector3.down;
        else if (xDown && x == 1)
            dirVec = Vector3.right;
        else if (xDown && x == -1)
            dirVec = Vector3.left;

        if(Input.GetButtonDown("Jump") && scanObject != null)
        {
            uiPost.SetActive(true);
            Rigidbody2D playerRigid = Player.GetComponent<Rigidbody2D>();
            playerRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        }


        void FixedUpdate()
        {
            Vector2 moveVec = isHorizonMove ? new Vector2(x, 0) : new Vector2(0, y);
            rigid.velocity = moveVec * 1.2f * Speed;

            Debug.DrawRay(rigid.position, dirVec * 0.7f,new Color(0,1,0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("object"));

            if (rayHit.collider != null)
                scanObject = rayHit.collider.gameObject;
            else
                scanObject = null;


        }

    }   


}