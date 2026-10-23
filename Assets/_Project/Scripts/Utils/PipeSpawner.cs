using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private int poolSize = 5;       // Достаточный размер пула
    [SerializeField] private float minY = -1.7f;
    [SerializeField] private float maxY = 1.7f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float xPosition = 7f;

    private GameObject[] _pipePool;
    private int _currentIndex = 0;
    private float _timer;

    void Start()
    {
        // Создаем пул один раз при запуске
        _pipePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            _pipePool[i] = Instantiate(pipePrefab);
            _pipePool[i].SetActive(false);
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            SpawnFromPool();
        }
    }

    private void SpawnFromPool()
    {
        GameObject pipe = _pipePool[_currentIndex];

        // Задаем новую позицию
        float randomY = Random.Range(minY, maxY);
        pipe.transform.position = new Vector2(xPosition, randomY);

        // Активируем объект, если он был скрыт
        if (!pipe.activeSelf)
        {
            pipe.SetActive(true);
        }

        // Переходим к следующему элементу по кругу
        _currentIndex = (_currentIndex + 1) % poolSize;
    }
}