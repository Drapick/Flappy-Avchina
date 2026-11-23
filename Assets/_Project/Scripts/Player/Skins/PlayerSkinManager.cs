using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    public static PlayerSkinManager Instance { get; private set; }

    [Header("Точка крепления префаба на персонаже")]
    [SerializeField] private Transform characterContainer;

    [Header("Путь в папке Resources")]
    [SerializeField] private string skinsFolderPath = "Skins/";

    private GameObject currentSkinInstance;
    private CancellationTokenSource loadCts;

    public string CurrentEquippedSkin { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (characterContainer == null)
        {
            characterContainer = transform;
        }
    }

    private void Start()
    {
        // При старте загружаем скин из сохранения без повторной перезаписи на диск
        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            string savedSkin = SaveManager.Instance.Data.currentSkin;
            if (!string.IsNullOrEmpty(savedSkin))
            {
                EquipSkin(savedSkin, saveToFile: false);
            }
        }
    }

    private void OnDestroy()
    {
        CancelCurrentLoading();
    }

    /// <summary>
    /// Экипирует скин на модель персонажа
    /// </summary>
    /// <param name="skinName">Имя префаба в папке Resources</param>
    /// <param name="saveToFile">Нужно ли сразу записывать выбор в сохранение</param>
    public async void EquipSkin(string skinName, bool saveToFile = true)
    {
        if (string.IsNullOrEmpty(skinName))
        {
            Debug.LogWarning("[PlayerSkinManager] Имя скина пустое!");
            return;
        }

        // Если этот скин уже надет, ничего повторно не грузим
        if (CurrentEquippedSkin == skinName && currentSkinInstance != null)
        {
            if (saveToFile) SaveCurrentSkin(skinName);
            return;
        }

        CancelCurrentLoading();
        loadCts = new CancellationTokenSource();
        CancellationToken token = loadCts.Token;

        // 1. Асинхронно получаем префаб
        GameObject skinPrefab = await LoadSkinPrefabAsync(skinName, token);

        if (token.IsCancellationRequested || skinPrefab == null) return;

        // 2. Спавним на персонажа
        ClearContainer();
        currentSkinInstance = Instantiate(skinPrefab, characterContainer);
        currentSkinInstance.transform.localPosition = Vector3.zero;
        currentSkinInstance.transform.localRotation = Quaternion.identity;
        currentSkinInstance.transform.localScale = Vector3.one;

        CurrentEquippedSkin = skinName;

        // 3. Сохраняем, если требуется
        if (saveToFile)
        {
            SaveCurrentSkin(skinName);
        }
    }

    /// <summary>
    /// Универсальный метод загрузки префаба скина (можно использовать из других скриптов)
    /// </summary>
    public async Task<GameObject> LoadSkinPrefabAsync(string skinName, CancellationToken token)
    {
        string fullPath = $"{skinsFolderPath}{skinName}";
        ResourceRequest request = Resources.LoadAsync<GameObject>(fullPath);

        while (!request.isDone)
        {
            if (token.IsCancellationRequested) return null;
            await Task.Yield();
        }

        if (token.IsCancellationRequested) return null;

        GameObject prefab = request.asset as GameObject;
        if (prefab == null)
        {
            Debug.LogWarning($"[PlayerSkinManager] Префаб скина '{skinName}' не найден по пути: Resources/{fullPath}");
        }

        return prefab;
    }

    /// <summary>
    /// Отдельный метод сохранения состояния
    /// </summary>
    public void SaveCurrentSkin(string skinName)
    {
        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            SaveManager.Instance.Data.currentSkin = skinName;
            SaveManager.Instance.SaveGame();
            Debug.Log($"[PlayerSkinManager] Скин '{skinName}' успешно сохранен!");
        }
    }

    private void ClearContainer()
    {
        if (characterContainer == null) return;

        foreach (Transform child in characterContainer)
        {
            Destroy(child.gameObject);
        }
        currentSkinInstance = null;
    }

    private void CancelCurrentLoading()
    {
        if (loadCts != null)
        {
            loadCts.Cancel();
            loadCts.Dispose();
            loadCts = null;
        }
    }
}
