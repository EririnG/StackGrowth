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
        // 마우스 클릭시 마다 패킷 클래스를 이용해서 위치 정보를 서버에 전송
        if(Input.GetMouseButtonDown(0) == true)
        {
            simple_packet new_packet = new simple_packet();
            new_packet.mouseX = Input.mousePosition.x;
            new_packet.mouseY = Input.mousePosition.y; 
            client.send(new_packet);
        }
    }

    private void on_app_quit()
    {
        if(this.client_sock != null)
        {
            this .client_sock.Close();
            this.client_sock = null;
        }
    }

    public static void send(simple_packet packet)
    {
        if(client.instance.client_sock == null)
        {
            return;
        }
        byte[] send_date = simple_packet.to_byte_array(packet);
        byte[] pref_size = new byte[1];
        pref_size[0] = (byte)send_date.Length;
        client.instance.client_sock.Send(pref_size);
        client.instance.client_sock.Send(send_date);

        Debug.Log("Send Packet from Client : " + packet.mouseX.ToString() + "/" + packet.mouseY.ToString());
    }
}
