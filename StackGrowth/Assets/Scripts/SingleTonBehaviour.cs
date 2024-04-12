using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance = null;
    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if(_instance != null)
                {
                    var _newGameObject = new GameObject(typeof(T).ToString());
                    _instance = _newGameObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if( _instance != null )
        {
            _instance = this as T;
        }
        DontDestroyOnLoad(gameObject);
    }
}
