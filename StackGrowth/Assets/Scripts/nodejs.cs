using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System;
using Network;
using System.Collections.Generic;


public class TcpClient : MonoBehaviour
{
    private System.Net.Sockets.TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    private bool isConnected = false;
    private float updateInterval = 1f; // 데이터 업데이트 주기 (초)
    private int id;
    private GameObject userObject;
    private Vector2 playerPosition;
    public GameObject player;
    public List<GameObject> playerList;
    private void Start()
    {
        userObject = GameObject.Find("Player");
        playerPosition = userObject.transform.position;
        ConnectToServer();
        player = Resources.Load("PreFabs/player") as GameObject;
        playerList = new List<GameObject>();
    }

    private void ConnectToServer()
    {
        client = new System.Net.Sockets.TcpClient();
        client.Connect("127.0.0.1", 30303);
        if (client.Connected)
        {
            isConnected = true;
            stream = client.GetStream();
            //receiveThread = new Thread(new ThreadStart(ReceiveData));
            //receiveThread.Start();
            Debug.Log("서버에 연결되었습니다.");

            
            //sendMessage("30ussersss");
            // 주기적으로 데이터 받아오는 코루틴 시작
            //StartCoroutine(ReceiveDataRoutine());
        }
    }

    private void ReceiveData()
    {
        if (stream.DataAvailable)
        {
            byte[] data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            //string message = Encoding.UTF8.GetString(data, 0, bytesRead);
            ByteReader br = new ByteReader(data);
            int _protocol = br.ReadInt();
            Debug.Log("프로토콜 : " + _protocol);
            
            switch(_protocol)
            {
                case (int)protocol.s_NewUser:
                    int newID = br.ReadInt();
                    Vector2 newPos = br.ReadVector2();
                    GameObject newInstance = Instantiate(player, newPos, Quaternion.identity);
                    newInstance.GetComponent<AnotherUser>().id = newID;
                    newInstance.GetComponent<AnotherUser>().pos = newPos;
                    //playerList.Add(newInstance);
                    break;
                case (int)protocol.s_PlayerConnect:
                    id = br.ReadInt();
                    Debug.Log("유저 아이디 : " + id);
                    int userNum = br.ReadInt();
                    Debug.Log("접속한 유저 수 : " + userNum);
                    for(int i = 0; i < userNum; i++)
                    {
                        int userid = br.ReadInt();
                        Vector2 uservec2 = br.ReadVector2();
                        Debug.Log("vector : " + uservec2);
                        GameObject instance = Instantiate(player, uservec2, Quaternion.identity);
                        instance.GetComponent<AnotherUser>().id = userid;
                        instance.GetComponent<AnotherUser>().pos = uservec2;
                        //playerList.Add(instance);
                    }
                    SendMessage(SendUserPosition());
                    break;
                case (int)protocol.s_PlayerPosition:
                    int moveUserID = br.ReadInt();
                    Vector2 movePos = br.ReadVector2();
                    foreach(GameObject user in playerList) {
                        if(user.GetComponent<AnotherUser>().id == moveUserID)
                        {
                            user.transform.position = movePos; 
                            break;
                        }
                    }
                    break;
                //case (int)protocol.c_PlayerCheckConn:
                //    SendMessage(SendUserPosition());
                //    break;
            }

            //id = int.Parse(message);
            //Debug.Log("서버로부터 메시지를 받았습니다: " + message);
                        
        }
   
    }

    private IEnumerator ReceiveDataRoutine()
    {
        while (isConnected)
        {
            // 서버로부터 데이터 수신
            if (stream.DataAvailable)
            {
                byte[] data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string message = Encoding.UTF8.GetString(data, 0, bytesRead);
                Debug.Log("서버로부터 메시지를 받았습니다: " + message);
            }

            // 지정된 업데이트 주기만큼 대기
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void Update()
    {

        ReceiveData();


    }

    public void SendMessage(byte[] message)
    {
        if (isConnected)
        {
            //byte[] data = Encoding.UTF8.GetBytes(message);
            //byte[] data  = BitConverter.GetBytes(message);
            stream.Write(message, 0, message.Length);
            //Debug.Log("서버로 메시지를 보냈습니다: " + message);
        }
    }

    private void OnApplicationQuit()
    {
        isConnected = false;
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();
    }

    public byte[] SendUserPosition() 
    {
        byte[] data = new byte[1024];
        ByteWriter bw = new ByteWriter(data);
        bw.WriteInt((int)protocol.c_PlayerPosition);
        bw.WriteInt(id);
        playerPosition = gameObject.transform.position; 
        bw.WriteVector2(playerPosition);
        return data;
    }
    //{
    //    byte[] data = new byte[1024];
    //    byte[] user = Encoding.UTF8.GetBytes("user");
    //    ByteWriter bw = new ByteWriter(data);
    //    bw.WriteInt(30);
    //    Debug.Log(BitConverter.ToInt32(data));
    //    //Buffer.BlockCopy(user,0, data, bw.Cursor,user.Length);
    //    bw.WriteBytes(user);

    //    Debug.Log(Encoding.UTF8.GetString(user));
    //    return data;
    //}
}