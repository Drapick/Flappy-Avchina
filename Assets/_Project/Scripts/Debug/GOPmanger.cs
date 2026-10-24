using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOPmanger : MonoBehaviour
{
    public static  GOPmanger Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // (Опционально) Чтобы менеджер не удалялся при переходе между сценами:
            DontDestroyOnLoad(gameObject); 
        }
    }
}
