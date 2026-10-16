using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeLogic : MonoBehaviour
{
    [Header("Логика пермемещения")]

    [Tooltip("Скорость движения pipes")]
    public float speed;

    void Update()
    {
        // Движение
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
