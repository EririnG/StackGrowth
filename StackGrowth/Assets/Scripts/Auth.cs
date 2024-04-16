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

public class Auth : MonoBehaviour
{
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

    public string server_ip = "127.0.0.1";
    public int server_port = 50002;
    private TcpClient conn_sock;
    NetworkStream stream;

    // Start is called before the first frame update

    void Start()
    {
        conn_serv();
    }

    private void conn_serv()
    {
        try
        {
            conn_sock = new TcpClient(server_ip, server_port);
            stream = conn_sock.GetStream();
            Console.WriteLine("서버에 연결되었습니다.");
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

            Console.WriteLine("데이터를 서버로 전송했습니다.");
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

                Console.WriteLine("데이터를 서버로 전송했습니다.");

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

    int ReadData()
    {
        byte[] buffer = new byte[1024];
        stream.Read(buffer, 0, buffer.Length);
        string data = Encoding.UTF8.GetString(buffer);
        Debug.Log(data);
        return int.Parse(data);
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

    IEnumerator LoadNextSceneDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Topdown");
    }

}
