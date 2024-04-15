using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed;

    float x;
    float y;

    bool isHorizonMove;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Update()
    {
        //move Value
        x = Input.GetAxisRaw("Horizontal") * 2;
        y = Input.GetAxisRaw("Vertical") *2 ;

        //Check Button Down & Up
        bool xDown = Input.GetButtonDown("Horizontal");
        bool yDown = Input.GetButtonDown("Vertical");
        bool xUp = Input.GetButtonUp("Horizontal");
        bool yUp = Input.GetButtonUp("Vertical");

        //Check Horizontal Move
        if(xDown || yUp)
            isHorizonMove = true; 
        else if(yDown || xUp)
            isHorizonMove = false;

    }
    
    // Update is called once per frame
    void FixedUpdate()//물리 입력이 있을 때만 존재
    {
        Vector2 moveVec = isHorizonMove ? new Vector2(x, 0) : new Vector2(0, y);
        rigid.velocity = moveVec * Speed;
    }
}
