using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // 바이너리 포매터


[Serializable]
public class simple_packet
{
    public float mouseX = 0.0f;
    public float mouseY = 0.0f;

    // 보내기
    public static byte[] to_byte_array(simple_packet packet)
    {
        // 스트림 생성
        MemoryStream stream = new MemoryStream();

        // 스트림으로 건너온 패킷을 바이너리 포맷으로 묶어준다.
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, packet.mouseX); 
        formatter.Serialize(stream, packet.mouseY);

        return stream.ToArray();
    }

    // 받기
    public static simple_packet from_byte_array(byte[] input)
    {
        // 스트림 생성
        MemoryStream input_stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        simple_packet packet = new simple_packet();

        packet.mouseX = (float)formatter.Deserialize(input_stream);
        packet.mouseY = (float)formatter.Deserialize(input_stream);

        return packet;
    }
}
