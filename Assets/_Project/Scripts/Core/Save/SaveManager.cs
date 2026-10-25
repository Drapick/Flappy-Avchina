using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public SaveData Data { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log($"[SaveManager] Путь к файлу: {saveFilePath}");
        LoadGame();
    }

    public void SaveGame()
    {
        if (Data == null)
        {
            Data = new SaveData();
        }

        try
        {
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"[SaveManager] Файл записан: {saveFilePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveManager] Ошибка при сохранении: {ex.Message}");
        }
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                Data = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[SaveManager] Ошибка чтения, сбрасываем: {ex.Message}");
                Data = new SaveData();
                SaveGame();
            }
        }
        else
        {
            // Файла не было: создаем дефолтный объект и СРАЗУ записываем его на диск
            Data = new SaveData();
            SaveGame();
        }
    }
}