using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceChecker : MonoBehaviour
{
    public float checkDistance = 1.1f; // Длина луча
    public LayerMask surfaceLayer;    // Слой поверхности (например, Ground)
    public bool isGrounded;
    public AudioSource audioSource; 
    public AudioClip landSound;     

    private bool wasGrounded; // Для отслеживания момента приземления

    void Update()
    {
        // В 2D используем Physics2D.Raycast
        // Он возвращает RaycastHit2D
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, surfaceLayer);

        // В 2D проверка "попали ли мы в цель" идет через проверку коллайдера на null
        if (hit.collider != null)
        {
            isGrounded = true;
            Debug.DrawRay(transform.position, Vector2.down * checkDistance, Color.red);
        }
        else
        {
            isGrounded = false;
            Debug.DrawRay(transform.position, Vector2.down * checkDistance, Color.white);
        }

        // Логика звука: если сейчас на земле, а в прошлом кадре были в воздухе
        if (isGrounded && !wasGrounded)
        {
            if (audioSource != null && landSound != null)
            {
                audioSource.PlayOneShot(landSound);
            }
        }

        // Запоминаем состояние для следующего кадра
        wasGrounded = isGrounded;
    }
}

