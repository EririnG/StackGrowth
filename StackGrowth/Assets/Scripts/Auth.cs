using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using System.Text;
using TMPro;


public class Auth : MonoBehaviour
{
    [SerializeField]
    TMP_InputField id_field;
    [SerializeField]
    TMP_InputField pw_field;

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
            conn_sock = new TcpClient("127.0.0.1", 50001);
            Console.WriteLine("서버에 연결되었습니다.");
        }
        catch (Exception e)
        {
            Debug.Log("Client Connect Exception " + e);
        }
    }

    private void send_msg()
    {
        if (conn_sock == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = conn_sock.GetStream();
            string message = id_field.text;
            byte[] data = Encoding.UTF8.GetBytes(message);

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
        Debug.Log(id_field.text);
        Debug.Log(pw_field.text);
        send_msg();
    }

}
