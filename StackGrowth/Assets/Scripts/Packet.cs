using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;


enum PACKET_ID
{
    Register = 21,
    Login = 22,
}

public class Packet
{
    public PktHeader p_header;
    public PktBody p_body;
}
public class PktHeader
{
    public short total_size;
    public short pack_id;
    public char reserve;   
}

public class PktBody
{
    byte[] data;
}

public class LoginPack : PktBody
{
    public byte[] id;
    public byte[] pw;


}