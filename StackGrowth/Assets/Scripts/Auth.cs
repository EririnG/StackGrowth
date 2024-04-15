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

    public GameObject pw_check_panel;


    public string server_ip = "127.0.0.1";
    public int server_port = 50001;
    private TcpClient conn_sock;
    // Start is called before the first frame update
    void Start()
    {
        conn_serv();
    }

    private void conn_serv()
    {

        try
        {
            conn_sock = new TcpClient("127.0.0.1", 50002);
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
            NetworkStream stream = conn_sock.GetStream();

            string msg = 22 + "/" + login_id_field.text + "/" + login_pw_field.text;
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

    private void send_register_msg()
    {
        if(register_pw_field.text != register_pw_repeat_field.text)
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
                NetworkStream stream = conn_sock.GetStream();


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
        
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            
            Debug.Log("Send Packet from Client");
        }
    }

    public void Login()
    {
        send_login_msg();
    }

    public void Register()
    {
        send_register_msg();
    }

}
