using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProgrewer : MonoBehaviour
{
    [Header("Настройки прогрева")]
    [Tooltip("Задержка после загрузки кадра (в секундах)")]
    public float warmUpTime = 1.0f;

    [Header("Объекты для активации")]
    [Tooltip("Экран загрузки (выключится после прогрева)")]
    public GameObject loadingScreen;
    
    [Tooltip("Объекты, которые нужно ВКЛЮЧИТЬ после прогрева")]
    public GameObject[] objectsToActivate;

    void Start()
    {
        // Сразу проверяем, чтобы лишние объекты были выключены в начале
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Запускаем процесс прогрева
        StartCoroutine(WarmUpRoutine());
    }

    IEnumerator WarmUpRoutine()
    {
        // 1. Ждем завершения отрисовки первого кадра
        yield return new WaitForEndOfFrame();
        Debug.Log("Кадр отрисован, начинаем прогрев...");

        // 2. Ждем заданное время (тот самый "прогрев")
        yield return new WaitForSeconds(warmUpTime);

        // 3. Выключаем экран загрузки, если он есть
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }

        // 4. Включаем все нужные объекты (геймплей, игрока, врагов)
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(true);
        }

        Debug.Log("Прогрев завершен! Геймплей запущен.");
    }
}
