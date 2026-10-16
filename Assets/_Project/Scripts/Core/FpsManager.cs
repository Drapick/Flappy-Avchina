using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsManager : MonoBehaviour
{
    [SerializeField] int current_fps = 60;
    private void Awake()
    {
        Application.targetFrameRate = current_fps;
    }
}
