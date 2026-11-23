using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ConveyorSkinSlot : MonoBehaviour
{
    [SerializeField] private Transform previewContainer;
    [SerializeField] private string skinsFolderPath = "Skins/";

    public string CurrentSkinName { get; private set; }
    public int CurrentDataIndex { get; private set; } = -1;

    private GameObject currentInstance;
    private CancellationTokenSource loadCts;

    private void Awake()
    {
        if (previewContainer == null)
        {
            previewContainer = transform;
        }
        
        ClearContainer();
    }

    public async void SetupSkin(int dataIndex, string skinName)
    {
        CurrentDataIndex = dataIndex;

        if (CurrentSkinName == skinName && currentInstance != null) return;
        CurrentSkinName = skinName;

        loadCts?.Cancel();
        loadCts?.Dispose();
        loadCts = new CancellationTokenSource();
        CancellationToken token = loadCts.Token;

        ResourceRequest req = Resources.LoadAsync<GameObject>($"{skinsFolderPath}{skinName}");
        while (!req.isDone)
        {
            if (token.IsCancellationRequested) return;
            await Task.Yield();
        }

        if (token.IsCancellationRequested || req.asset == null) return;

        // Удаляем все старые модели перед спавном новой
        ClearContainer();

        GameObject prefab = req.asset as GameObject;
        currentInstance = Instantiate(prefab, previewContainer);
        currentInstance.transform.localPosition = Vector3.zero;
        currentInstance.transform.localRotation = Quaternion.identity;
        currentInstance.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Полная очистка контейнера от заглушек и предыдущих инстансов
    /// </summary>
    public void ClearContainer()
    {
        if (previewContainer == null) return;

        // Удаляем все дочерние объекты внутри контейнера
        for (int i = previewContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(previewContainer.GetChild(i).gameObject);
        }

        currentInstance = null;
    }

    private void OnDestroy()
    {
        loadCts?.Cancel();
        loadCts?.Dispose();
    }
}
