using TMPro;
using UnityEngine;

public class FlAvManager : MonoBehaviour
{
    public static FlAvManager Instance { get; private set; }

    [Header("Настройки уровня")]
    public int currentLevelID; // 0 для тестового уровня, 1 для 1-го и т.д.

    [Header("Объекты")]
    public GameObject LoseWindow;
    public GameObject Player;

    [Header("Интерфейс")]
    [SerializeField] private int score;
    [SerializeField] private int bestScore;
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI bestScoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitAndLoadHighScore();
    }

    // Экран смерти — здесь фиксируем рекорд и сохраняем на диск
    public void Lose()
    {
        if (LoseWindow) LoseWindow.SetActive(true);
        if (Player) Player.SetActive(false);

        // Записываем на диск только если действительно был побит рекорд
        if (score > bestScore)
        {
            bestScore = score;
            SaveLevelHighScore();
        }
    }
    
    public void AddScore()
    {
        score++;
        UpdateScoreUI();

        // Обновляем визуальный рекорд в реальном времени
        if (score > bestScore)
        {
            UpdateHighScoreUI(score);
        }
    }

    private void UpdateScoreUI()
    {
        foreach (var text in scoreTexts)
        {
            if (text) text.SetText("{0}", score);
        }
    }

    private void UpdateHighScoreUI(int valueToDisplay)
    {
        if (bestScoreText) bestScoreText.SetText("{0}", valueToDisplay);
    }

    // Загрузка рекорда при старте
    private void InitAndLoadHighScore()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("[FlAvManager] SaveManager.Instance не найден на сцене!");
            return;
        }

        var highScores = SaveManager.Instance.Data.levelHighScores;

        // Создаем ячейку под текущий уровень, если ее нет
        bool isNewSlotCreated = false;
        while (highScores.Count <= currentLevelID)
        {
            highScores.Add(0);
            isNewSlotCreated = true;
        }

        if (isNewSlotCreated)
        {
            SaveManager.Instance.SaveGame();
        }

        bestScore = highScores[currentLevelID];
        UpdateHighScoreUI(bestScore);
    }

    // Единственная точка записи рекорда в JSON
    private void SaveLevelHighScore()
    {
        if (SaveManager.Instance == null) return;

        var highScores = SaveManager.Instance.Data.levelHighScores;

        while (highScores.Count <= currentLevelID)
        {
            highScores.Add(0);
        }

        highScores[currentLevelID] = bestScore;
        SaveManager.Instance.SaveGame();
    }
}
