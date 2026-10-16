using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorReflection : MonoBehaviour
{
    public Transform target;      // Ссылка на основного персонажа
    public float mirrorHeight;   // Y-координата линии пола

    void LateUpdate()
    {
        if (target != null)
        {
            // --- 1. Зеркальная Позиция ---
            float distanceToMirror = target.position.y - mirrorHeight;
            transform.position = new Vector3(
                target.position.x, 
                mirrorHeight - distanceToMirror, 
                target.position.z
            );

            // --- 2. Зеркальная Ротация ---
            // Копируем вращение и инвертируем его по оси Z
            Vector3 targetRotation = target.eulerAngles;
            // Чтобы отражение наклонялось в нужную сторону, 
            // мы просто меняем знак угла поворота Z
            transform.eulerAngles = new Vector3(
                targetRotation.x, 
                targetRotation.y, 
                -targetRotation.z 
            );
        }
    }
}

