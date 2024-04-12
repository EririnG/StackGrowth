using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; // ���̳ʸ� ������


[Serializable]
public class simple_packet
{
    public float mouseX = 0.0f;
    public float mouseY = 0.0f;

    // ������
    public static byte[] to_byte_array(simple_packet packet)
    {
        // ��Ʈ�� ����
        MemoryStream stream = new MemoryStream();

        // ��Ʈ������ �ǳʿ� ��Ŷ�� ���̳ʸ� �������� �����ش�.
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, packet.mouseX); 
        formatter.Serialize(stream, packet.mouseY);

        return stream.ToArray();
    }

    // �ޱ�
    public static simple_packet from_byte_array(byte[] input)
    {
        // ��Ʈ�� ����
        MemoryStream input_stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        simple_packet packet = new simple_packet();

        packet.mouseX = (float)formatter.Deserialize(input_stream);
        packet.mouseY = (float)formatter.Deserialize(input_stream);

        return packet;
    }
}
