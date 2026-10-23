using UnityEngine;

public class GrassRandomizer : MonoBehaviour
{
    [Header("Настройки травы")]
    [Tooltip("Массив объектов травы. Если оставить пустым, найдет всех детей автоматически")]
    [SerializeField] private Transform[] grassElements;

    [Header("Диапазон смещения по локальной оси X")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;

    private void Awake()
    {
        // Если трава не назначена вручную в инспекторе, собираем всех прямых потомков
        if (grassElements == null || grassElements.Length == 0)
        {
            grassElements = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                grassElements[i] = transform.GetChild(i);
            }
        }
    }

    /// <summary>
    /// Рандомит позицию травы строго для текущего GrassHub
    /// </summary>
    public void RepositionGrass()
    {
        if (grassElements == null) return;

        foreach (var grass in grassElements)
        {
            if (grass == null) continue;

            float randomX = Random.Range(minX, maxX);

            Vector3 localPos = grass.localPosition;
            localPos.x = randomX;
            grass.localPosition = localPos;
        }
    }
}