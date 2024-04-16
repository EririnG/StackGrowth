using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    Animator anim; // Animation�� control �ϱ� ���ؼ�
    Rigidbody2D rigid;
    Vector3 dirVec;
    GameObject scanObject;

    public GameObject Player; 
    public GameObject uiPost; //public���� �������ָ� unity���� �巡�׾� ��� ����


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

        //������ ������ 0 ,-1 ,1
        bool xDown = Input.GetButtonDown("Horizontal");
        bool yDown = Input.GetButtonDown("Vertical");
        bool xUp = Input.GetButtonUp("Horizontal");
        bool yUp = Input.GetButtonUp("Vertical");

        //�����̵����� Ȯ��
        if (xDown)
        {
            isHorizonMove = true;
        }
        else if (yDown)
        {
            isHorizonMove = false;
        }
        else if (xUp || yUp)//����,������ ���ÿ� ������ ������ ���� ���Ѵ�.
        {
            isHorizonMove = x != 0;
        }
        
        //if���� ���ٸ� ��� �ٽ� �����Ų��.-> �� , �ִϸ��̼��� ó�� �����ӿ��� ���� ���·� ������.
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