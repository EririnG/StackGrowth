using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAction : MonoBehaviour
{

    Animator anim;//Animation control �ϱ� ���ؼ�
    Rigidbody2D rigid;
    Vector3 dirVec;
    GameObject scanObject;
    public GameObject Player;
    public GameObject uiPost;

    public float Speed;
    float x;
    float y;
    bool isHorizonMove;
    //public GameManager manager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    // Start is called before the first frame update
    void Update()
    {
        //move Value
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        //Check Button Down & Up
        bool xDown = Input.GetButtonDown("Horizontal");
        bool yDown = Input.GetButtonDown("Vertical");
        bool xUp = Input.GetButtonUp("Horizontal");
        bool yUp = Input.GetButtonUp("Vertical");

        //Check Horizontal Move
        if(xDown)
            isHorizonMove = true; 
        else if(yDown)
            isHorizonMove = false;
        else if(xUp || yUp)
        {
            isHorizonMove = x != 0;//�� �Ʒ� ���ÿ� ������ ����,������ ���ÿ� ������ ��
        }

        //Animation, if���� ���ٸ� ��� �ٽ� ������Ѿ���-> �ִϸ��̼� �ʱ� �����Ӹ� ��� ����ȴ�.
        if (anim.GetInteger("xAxisRaw") != x)//�ش� ���� X�� ���ٸ�(��ȭ�� ���� ��)
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

        //Direction, �츮�� �ٷκ��� �ִ� ����
        if(yDown && y == 1)
            dirVec = Vector3.up;
        else if (yDown && y == -1)
            dirVec = Vector3.down;
        else if(xDown && x == -1)
            dirVec = Vector3.left;
        else if(xDown && x == 1)
            dirVec = Vector3.right;

        //Scanf Object
        if(Input.GetButtonDown("Jump") && scanObject != null)
        {
            uiPost.SetActive(true);
            Rigidbody2D playerRigid = Player.GetComponent<Rigidbody2D>();
            playerRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }



    // Update is called once per frame
    void FixedUpdate()//���� �Է��� ���� ���� ����
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(x, 0) : new Vector2(0, y);
        rigid.velocity = moveVec * 1.2f * Speed;

        // Ray, ��ĵ �� �� ���,DrawRay���� �غ��� RayCast ���� �� ����.
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        // LayerMask.GetMask("Object") Object�� Scan �����ϴ�.
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;

        
    }

    public void melt()
    {
        Rigidbody2D playerRigid = Player.GetComponent<Rigidbody2D>();
        playerRigid.constraints = RigidbodyConstraints2D.None;
        playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;

    }
}
