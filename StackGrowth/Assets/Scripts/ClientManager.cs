using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

//public class ClientManager : SingleTonBehaviour<ClientManager>
public class ClientManager : MonoBehaviour
{
    // login
    [SerializeField]
    TMP_InputField login_id_field;
    [SerializeField]
    TMP_InputField login_pw_field;
    [SerializeField]
    TMP_InputField register_nick_field;
    [SerializeField]
    TMP_InputField register_id_field;
    [SerializeField]
    TMP_InputField register_pw_field;
    [SerializeField]
    TMP_InputField register_pw_repeat_field;

    public GameObject suc_reg_panel;
    public GameObject suc_login_panel;
    public GameObject pw_check_panel;
    public GameObject nick_check_panel;
    public GameObject id_check_panel;
    public GameObject id_login_check_panel;


    // game
    [SerializeField]
    TMP_InputField post_title;
    [SerializeField]
    TMP_InputField post_author;
    [SerializeField]
    TMP_InputField post_content;

    public GameObject suc_post_panel;


    private spawner postManagerInstance;
    // sever

    public string server_ip = "127.0.0.1";
    public int server_port = 50002;
    private TcpClient conn_sock;
    NetworkStream stream;

    // Start is called before the first frame update

    void Start()
    {
        conn_serv();
        GameObject postManagerObject = GameObject.Find("PostManager");
        postManagerInstance = postManagerObject.GetComponent<spawner>();
    }

    private void conn_serv()
    {
        try
        {
            conn_sock = new TcpClient(server_ip, server_port);
            stream = conn_sock.GetStream();
            Console.WriteLine("������ ����Ǿ����ϴ�.");
        }
        catch (Exception e)
        {
            Debug.Log("Client Connect Exception " + e);
        }
    }

    private void send_login_msg()
    {
        if (conn_sock == null)
        {
            return;
        }
        try
        {
            string msg = 22 + "/" + login_id_field.text + "/" + login_pw_field.text;
            byte[] data = Encoding.UTF8.GetBytes(msg);

            stream.Write(data, 0, data.Length);

            Console.WriteLine("�����͸� ������ �����߽��ϴ�.");
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception " + e);
        }

        int res = ReadData();
        Debug.Log(res);
        switch (res)
        {
            case 0:
                suc_login_panel.gameObject.SetActive(true);
                StartCoroutine(LoadNextSceneDelay(3f));
                break;
            case 1:
                id_login_check_panel.gameObject.SetActive(true);
                break;
            case 2:
                pw_check_panel.gameObject.SetActive(true);
                break;
            default:
                break;
        }

    }

    private void send_register_msg()
    {
        if (register_pw_field.text != register_pw_repeat_field.text)
        {
            pw_check_panel.gameObject.SetActive(true);
        }
        else
        {
            if (conn_sock == null)
            {
                return;
            }
            try
            {
                string msg = 21 + "/" + register_nick_field.text + "/" + register_id_field.text + "/" + register_pw_field.text;
                byte[] data = Encoding.UTF8.GetBytes(msg);

                stream.Write(data, 0, data.Length);

                Console.WriteLine("�����͸� ������ �����߽��ϴ�.");

                //stream.Close();
                //conn_sock.Close();
            }
            catch (SocketException e)
            {
                Debug.Log("Socket Exception " + e);
            }
        }

        int res = ReadData();
        Debug.Log(res);
        switch (res)
        {
            case 0:
                suc_reg_panel.gameObject.SetActive(true); 
                break;
            case 1:
                nick_check_panel.gameObject.SetActive(true);
                break;
            case 2:
                id_check_panel.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void send_post_msg()
    {
        if (conn_sock == null)
        {
            return;
        }
        try
        {
            string msg = 23 + "/" + post_title.text + "/" + post_author.text + "/" + post_content.text;
            byte[] data = Encoding.UTF8.GetBytes(msg);

            stream.Write(data, 0, data.Length);

            Console.WriteLine("�����͸� ������ �����߽��ϴ�.");
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception " + e);
        }
        int res = ReadData();
        if (res == 0)
        {
            suc_post_panel.gameObject.SetActive(true);
            post_title.text = "";
            post_author.text = "";
            post_content.text = "";
        }
        else
            Debug.Log("���� �߸���");
    }

    public void open_post()
    {
        if (conn_sock == null)
        {
            return;
        }
        try
        {
            string msg = "24";
            byte[] data = Encoding.UTF8.GetBytes(msg);

            stream.Write(data, 0, data.Length);

            Console.WriteLine("�����͸� ������ �����߽��ϴ�.");
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception " + e);
        }
        //int res = ReadData();
        // make �κ�
        
        string read_data = ReadData_str();
        string[] parts = read_data.Split('/');
        Debug.Log(parts.Length);
        read_data = "";
        for(int i = 1; i < parts.Length; i++)
        {
            read_data = read_data + parts[i - 1] + "/";
            if (i % 4 == 0)
            {
                postManagerInstance.spawn(read_data);
                Debug.Log(read_data);
                read_data = "";
            }
                
        }
    }


    int ReadData()
    {
        byte[] buffer = new byte[1024];
        stream.Read(buffer, 0, buffer.Length);
        string data = Encoding.UTF8.GetString(buffer);
        Debug.Log(data);
        return int.Parse(data);
    }
    string ReadData_str()
    {
        byte[] buffer = new byte[1024];
        stream.Read(buffer, 0, buffer.Length);
        string data = Encoding.UTF8.GetString(buffer);
        Debug.Log(data);
        return data;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        send_login_msg();
    }

    public void Register()
    {
        send_register_msg();
    }

    public void Post()
    {
        send_post_msg();
    }


    IEnumerator LoadNextSceneDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Topdown");
    }

}
