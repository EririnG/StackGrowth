using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class client : SingleTonBehaviour<client>
{
    public string server_ip = "127.0.0.1";
    public int m_Port = 50001;
    Socket client_sock = null;

    private void Start()
    {
        this.client_sock = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

        IPAddress server_address = IPAddress.Parse(this.server_ip);
        IPEndPoint server_end_point = new IPEndPoint(server_address, m_Port);

        // 서버 연결 요청
        try
        {
            Debug.Log("Connecting to Server");
            this.client_sock.Connect(server_end_point);
        }
        catch(SocketException e)
        {
            Debug.Log("Connection Failed : " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void on_app_quit()
    {
        if(this.client_sock != null)
        {
            this .client_sock.Close();
            this.client_sock = null;
        }
    }

    //public static void send(SimplePacket packet)
    //{

    //}
}
