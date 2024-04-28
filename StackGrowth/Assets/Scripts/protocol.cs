using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public enum protocol
    {
        s_NewUser = 29,
        s_PlayerConnect = 30,
        c_PlayerCheckConn = 31,
        c_PlayerPosition = 32,
        s_PlayerPosition = 33,
    }

}