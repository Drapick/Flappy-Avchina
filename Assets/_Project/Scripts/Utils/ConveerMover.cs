using System.Collections;
using UnityEngine;

public class ConveerMover : MonoBehaviour
{
    public float speed = 15f;
    public AudioSource ConveerSFX_source;
    public AudioClip ConveerSFX;
    // Вектор одного "шага" (разница между твоими точками)
    // Z: 125 - 150 = -25
    // Y: -30 - 0 = -30
    private Vector3 stepVector = new Vector3(0, -60f, -50f);
    
    private Vector3 targetPos;
    private Coroutine moveCoroutine;
    private bool isMoving = false;

    void Start()
    {
        // Начальная позиция
        transform.position = new Vector3(0, 0, 150);
        targetPos = transform.position;
    }

    // Листаем дальше по вектору (вниз)
    public void MoveNext()
    {
        if (isMoving) return;
        targetPos += stepVector; // Добавляем еще один сегмент к цели
        StartMove();
    }

    // Листаем назад по вектору (вверх)
    public void MoveBack()
    {
        if (isMoving) return;

        Vector3 nextPos = targetPos - stepVector;
        if (nextPos.y > 0 || nextPos.z > 150) return;

        targetPos -= stepVector; // Вычитаем сегмент из цели
        StartMove();
    }

    private void StartMove()
    {
        ConveerSFX_source.PlayOneShot(ConveerSFX);
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPos, 
                speed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
}
