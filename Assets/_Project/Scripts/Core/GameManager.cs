using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    // Статическая ссылка на экземпляр, доступная из других скриптов
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Проверка: если экземпляр уже существует, удаляем дубликат
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // (Опционально) Чтобы менеджер не удалялся при переходе между сценами:
            DontDestroyOnLoad(gameObject); 
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
