using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeLogic : MonoBehaviour
{
    [Header("Логика пермемещения")]
    [Tooltip("Скорость движения pipes")]
    public float speed;

    private Rigidbody2D rb;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = new Vector2(-speed, 0);
    }
}
