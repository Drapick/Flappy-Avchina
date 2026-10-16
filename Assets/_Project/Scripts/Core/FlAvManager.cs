using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

[System.Serializable]
public class SaveData {
    // Список рекордов, где индекс = номер уровня
    public List<int> levelHighScores = new List<int>();
}

public class FlAvManager : MonoBehaviour
{
    public static FlAvManager Instance { get; private set; }

    [Header("Настройки уровня")]
    public int currentLevelID; // Укажи 0 для 1-го уровня, 1 для 2-го и т.д.

    [Header("Объекты")]
    public GameObject LoseWindow;
    public GameObject Player;
    
    [Header("Интерфейс")]
    [SerializeField] int score;
    [SerializeField] int bestScore;
    public TextMeshProUGUI[] scoreTexts; 
    public TextMeshProUGUI bestScoreText;

    private string savePath;
    private SaveData data = new SaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
            Debug.Log("Файл сохранения здесь: " + savePath);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Сцены
    public void Lose()
    {
        LoseWindow.SetActive(true);
        Player.SetActive(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Очки
    
    /// <summary>
    /// Добавляет 1 очко к текущему счету и автоматически перезаписывает JSON-сейв, если побит рекорд.
    /// </summary>
    public void AddScore() {
        score++;
        UpdateScoreUI();

        if (score > bestScore) {
            bestScore = score;
            UpdateHighScoreUI();
            SaveGame(); 
        }
    }

    private void UpdateScoreUI() {
        foreach (var text in scoreTexts) {
            if (text) text.text = score.ToString();
        }
    }

    private void UpdateHighScoreUI() {
        if (bestScoreText) bestScoreText.text = bestScore.ToString();
    }

    // --- ЛОГИКА JSON ---

    public void SaveGame() {
        // Убеждаемся, что в списке есть место под текущий уровень
        while (data.levelHighScores.Count <= currentLevelID) {
            data.levelHighScores.Add(0);
        }

        // Обновляем значение именно для этого уровня
        data.levelHighScores[currentLevelID] = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame() {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<SaveData>(json);

            // Достаем рекорд текущего уровня, если он есть
            if (currentLevelID < data.levelHighScores.Count) {
                bestScore = data.levelHighScores[currentLevelID];
            } else {
                bestScore = 0;
            }
        } else {
            bestScore = 0;
        }
        UpdateHighScoreUI();
    }
}
