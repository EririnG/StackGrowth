using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject scanObject;

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
    }

}
