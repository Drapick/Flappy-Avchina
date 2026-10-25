using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunOcclusion : MonoBehaviour
{
    [Header("Настройки затемнения")]
    [Range(0f, 1f)]
    [SerializeField] private float occludedFactor = 0.15f; // затемнение на -85%
    [SerializeField] private float fadeSpeed = 4f;

    private Light2D[] _allLights;
    private float[] _baseIntensities;
    private float _currentFactor = 1f;
    private float _targetFactor = 1f;
    private int _occludingCollidersCount = 0;

    private void Awake()
    {
        // Находит Light 2D на самом солнце и во всех дочерних godrays
        _allLights = GetComponentsInChildren<Light2D>(true);
        _baseIntensities = new float[_allLights.Length];

        // Запоминаем исходную яркость каждого источника
        for (int i = 0; i < _allLights.Length; i++)
        {
            _baseIntensities[i] = _allLights[i].intensity;
        }
    }

    private void Update()
    {
        if (Mathf.Abs(_currentFactor - _targetFactor) > 0.002f)
        {
            _currentFactor = Mathf.MoveTowards(_currentFactor, _targetFactor, fadeSpeed * Time.deltaTime);

            for (int i = 0; i < _allLights.Length; i++)
            {
                if (_allLights[i] != null)
                {
                    _allLights[i].intensity = _baseIntensities[i] * _currentFactor;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (IsPipe(other))
        {
            _occludingCollidersCount++;
            _targetFactor = occludedFactor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPipe(other))
        {
            _occludingCollidersCount = Mathf.Max(0, _occludingCollidersCount - 1);

            // Возвращаем исходную яркость, только когда солнце покинули обе части трубы
            if (_occludingCollidersCount == 0)
            {
                _targetFactor = 1f;
            }
        }
    }

    private bool IsPipe(Collider2D col)
    {
        return col.CompareTag("PipePart") || col.CompareTag("PipePartTwo") || col.CompareTag("TANKA");
    }
}
