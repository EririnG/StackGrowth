using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class client_interface:MonoBehaviour
{
    private TcpClient conn_sock;
    // Start is called before the first frame update
    void Start()
    {
        conn_tcp_sock();
    }

    private void conn_tcp_sock()
    {
        try
        {
            conn_sock = new TcpClient("127.0.0.1", 50001);
            Console.WriteLine("서버에 연결되었습니다.");
        }
        catch(Exception e)
        {
            Debug.Log("Client Connect Exception " + e);
        }
    }

    private void dsadsa_send()
    {
        if(conn_sock == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = conn_sock.GetStream();
            string message = "11";
            byte[] data = Encoding.UTF8.GetBytes(message);

            stream.Write(data, 0, data.Length);
            Console.WriteLine("데이터를 서버로 전송했습니다.");

            //stream.Close();
            //conn_sock.Close();
        }
        catch(SocketException e)
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
}
