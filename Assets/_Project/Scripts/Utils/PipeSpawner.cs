using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipe_prefab;      // Префаб
    public float minY;             // Минимум по Y
    public float maxY;             // Максимум по Y
    public float spawnInterval = 2f; // Интервал спавна
    public float xPosition = 10f;  // Точка спавна по X

    void Start()
    {
        // Запускаем повторение метода Spawn через каждые spawnInterval секунд
        InvokeRepeating("Spawn", 0f, spawnInterval);
    }

    void Spawn()
    {
        // Генерируем случайную высоту
        float randomY = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(xPosition, randomY);

        // Создаем префаб
        GameObject newPipe = Instantiate(pipe_prefab, spawnPos, Quaternion.identity);
        
        // Удаляем спустя время
        Destroy(newPipe, 6f);
    }
}
