using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLogic : MonoBehaviour
{
    [Header("Логика перемещения")]
    [Tooltip("Скорость движения ground")]
    public float speed;
    [Tooltip("Корды для телепорта ground для бесшовной работы")]
    public float StartX;
    public float EndX;

    [Header("Ссылки на траву")]
    [Tooltip("Список всех GrassHub внутри этого мувера")]
    [SerializeField] private GrassRandomizer[] grassRandomizers;

    private void Awake()
    {
        if (grassRandomizers == null || grassRandomizers.Length == 0)
        {
            grassRandomizers = GetComponentsInChildren<GrassRandomizer>();
        }
    }

    private void Start()
    {
        // Инициализируем случайные позиции травы сразу при запуске игры
        RepositionAllGrass();
    }
    
    void Update()
    {
        GroundMover();
    }

    private void GroundMover()
    {

        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Телепорт в исходную позицию
        if (transform.position.x <= EndX)
        {

            float overlap = EndX - transform.position.x;
            

            transform.position = new Vector2(StartX - overlap, transform.position.y);

            // Перемешиваем траву на всех ground_1 и ground_2 текущего мувера
            RepositionAllGrass();
        }
    }

    private void RepositionAllGrass()
    {
        if (grassRandomizers == null) return;

        foreach (var randomizer in grassRandomizers)
        {
            if (randomizer != null)
            {
                randomizer.RepositionGrass();
            }
        }
    }
}