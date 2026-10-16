using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FLAvController : MonoBehaviour
{
    [Header("Настройки физики")]
    [Tooltip("Сила прыжка птицы")]
    public float jumpForce = 5f;
    
    [Header("Ссылки на компоненты")]
    [Tooltip("Скрипт анимации поворота")]
    public FlappyAvchinaRotation birdAnimation;
    public GameObject GetReadyCanv;
    
    [Header("Звуковые эффекты")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip scoreSound;

    private Rigidbody2D rb;

    void Awake() 
    {
        // Инициализируем Rigidbody один раз при запуске
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        // Объединяем проверку ввода: Мышь (0) или Пробел
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Передаем вертикальную скорость для анимации наклона
        if (birdAnimation != null)
        {
            birdAnimation.ApplyRotation(rb.velocity.y);
        }
    }

    private void Jump()
    {
        // Сбрасываем скорость перед прыжком, чтобы прыжки всегда были одинаковыми
        rb.velocity = Vector2.up * jumpForce;
        
        // Запускаем анимацию взлета
        if (birdAnimation != null) birdAnimation.StartRotation();
        
        // Воспроизводим звук, если он назначен
        if (audioSource && jumpSound) audioSource.PlayOneShot(jumpSound);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Проверяем столкновение по тегам
        string tag = other.gameObject.tag;

        if (tag == "PipePart" || tag == "PipePartTwo" || tag == "ground")
        {
            GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D electoral)
    {
        // Здесь обычно логика пролета через трубы (начисление очков)
        if (electoral.CompareTag("ScoreTrigger"))
        {
            AddScore();
            GetReadyCanv.SetActive(false);
        }
    }

    private void AddScore()
    {
        // Логика начисления очков
        if (audioSource && scoreSound) audioSource.PlayOneShot(scoreSound);
        FlAvManager.Instance.AddScore(); // Пример вызова
    }

    private void GameOver()
    {
        Debug.Log("Игра окончена!");
        FlAvManager.Instance.Lose();
    }
}
