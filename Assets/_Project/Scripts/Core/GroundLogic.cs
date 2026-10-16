using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLogic : MonoBehaviour
{
    [Header("Логика пермемещения")]

    [Tooltip("Скорость движения ground")]
    public float speed;
    [Tooltip("Корды для телепорта ground для бесшовной работы")]
    public float StartX;
    public float EndX;

    void Update()
    {
        GroundMover();
    }

    private void GroundMover()
    {
        // Движение
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Телепорт в исходную позицию с учетом погрешности
        if (transform.position.x <= EndX)
        {
            // Вычисляем, насколько объект пролетел мимо EndX
            float overlap = EndX - transform.position.x;
            
            // Перемещаем в StartX, вычитая эту погрешность
            transform.position = new Vector2(StartX - overlap, transform.position.y);
        }
    }
}
