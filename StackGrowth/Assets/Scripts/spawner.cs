using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class spawner : MonoBehaviour
{
    public static spawner mini_Instance;
    public GameObject post;
    public Transform spawn_point;
    int spacing = 10;

    public TextMeshProUGUI title;
    public TextMeshProUGUI author;
    public TextMeshProUGUI id;
    private ClientManager clientManagerInstance;

    // 게시글용
    public GameObject post_main;
    public GameObject panel;
    public Transform spawn_point_main;

    // Start is called before the first frame update
    private void Awake()
    {
        mini_Instance = this;
        
    }
    void Start()
    {
        GameObject clientManagerObject = GameObject.Find("ClientManager");
        clientManagerInstance = clientManagerObject.GetComponent<ClientManager>();
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
        string[] parts = buf.Split('/');
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
        title = instance.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        author = instance.transform.Find("Author").GetComponent<TextMeshProUGUI>();
        id = instance.transform.Find("id").GetComponent<TextMeshProUGUI>();
        id.text = parts[0];
        title.text = parts[1];
        author.text = parts[2];
        Button button = instance.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => clientManagerInstance.make_post(instance));
        button.onClick.AddListener(() => panel.SetActive(true));
    }

    public void spawn_post(string buf)
    {
        string[] parts = buf.Split('/');
        GameObject instance = Instantiate(post_main, spawn_point_main);
        RectTransform newPostTransform = instance.GetComponent<RectTransform>();
        newPostTransform.SetParent(spawn_point_main, false);

        title = instance.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        author = instance.transform.Find("Author").GetComponent<TextMeshProUGUI>();
        id = instance.transform.Find("Content").GetComponent<TextMeshProUGUI>();
        Button button = instance.GetComponentInChildren<Button>();
        //button.onClick.AddListener(clientManagerInstance.make_post);

        id.text = parts[3];
        title.text = parts[1];
        author.text = parts[2];
    }

}
