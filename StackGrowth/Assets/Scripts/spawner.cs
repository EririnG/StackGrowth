using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawner : MonoBehaviour
{
    public static spawner Instance;
    public GameObject post;
    public Transform spawn_point;
    int spacing = 10;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    spawn();
        //}
    }

    public void spawn(string buf)
    {
        RectTransform lastPostTransform = (spawn_point.childCount > 0) ? spawn_point.GetChild(spawn_point.childCount - 1).GetComponent<RectTransform>() : null;
        GameObject instance = Instantiate(post, spawn_point);
        RectTransform newPostTransform = instance.GetComponent<RectTransform>();
        if (lastPostTransform != null)
        {
            float posY = lastPostTransform.localPosition.y - lastPostTransform.rect.height - spacing;
            newPostTransform.localPosition = new Vector3(0f, posY, 0f);
        }
        else
            newPostTransform.localPosition = Vector3.zero;

        newPostTransform.SetParent(spawn_point, false);

    }

    void setTitle()
    {

    }
}
