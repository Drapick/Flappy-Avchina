using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.Universal;

public class PostProcessingUIController : MonoBehaviour
{
    private Button _button;
    private TextMeshProUGUI _buttonText;
    private UniversalAdditionalCameraData _cameraData;

    // Запоминаем текущий выбор игрока (по умолчанию включен)
    private bool _isPostFxEnabled = true;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (_button != null)
            _button.onClick.AddListener(TogglePostProcessing);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Первичный поиск камеры на стартовой сцене
        StartCoroutine(FindCameraNextFrame());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Сбрасываем мертвую ссылку от прошлой сцены
        _cameraData = null;

        // Ждем 1 кадр, пока Unity полностью активирует камеру новой сцены
        StartCoroutine(FindCameraNextFrame());
    }

    private IEnumerator FindCameraNextFrame()
    {
        // Пропускаем текущий кадр инициализации сцены
        yield return null;

        Camera cam = Camera.main;
        if (cam == null)
            cam = FindFirstObjectByType<Camera>();

        if (cam != null)
        {
            _cameraData = cam.GetUniversalAdditionalCameraData();

            // Применяем текущее состояние к новой камере (чтобы не сбрасывалось между сценами)
            _cameraData.renderPostProcessing = _isPostFxEnabled;
        }

        UpdateVisuals();
    }

    public void TogglePostProcessing()
    {
        // Если ссылка вдруг потеряна, пробуем найти камеру прямо сейчас
        if (_cameraData == null)
        {
            Camera cam = Camera.main != null ? Camera.main : FindFirstObjectByType<Camera>();
            if (cam != null)
                _cameraData = cam.GetUniversalAdditionalCameraData();
        }

        if (_cameraData == null) return;

        // Инвертируем состояние
        _isPostFxEnabled = !_isPostFxEnabled;
        _cameraData.renderPostProcessing = _isPostFxEnabled;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_buttonText == null) return;

        if (_cameraData == null)
        {
            _buttonText.text = "Post-FX: NO CAM";
            return;
        }

        _buttonText.text = _isPostFxEnabled ? "Post-FX: ON" : "Post-FX: OFF";
    }
}
