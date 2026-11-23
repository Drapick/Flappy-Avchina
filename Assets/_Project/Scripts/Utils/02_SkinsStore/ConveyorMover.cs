using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConveyorMover : MonoBehaviour
{
    public float speed = 15f;
    public float startPosY = 10f;
    public AudioSource ConveyorSFX_source;
    public AudioClip ConveyorSFX;

    [Header("Слоты конвейера (4 штуки снизу вверх)")]
    [SerializeField] private ConveyorSkinSlot[] skinSlots; 
    
    [SerializeField] private ShopSelectButton shopButton;
    private List<string> allSkinNames => SkinData.AllSkinNames;

    [SerializeField] private Vector3 rawDirection = new Vector3(0f, -60f, -50f);
    [SerializeField] private float stepLength = 80f;
    private Vector3 stepVector;
    private Vector3 targetPos;
    
    private Coroutine moveCoroutine;
    private bool isMoving = false;

    private const float ThresholdGlobalY = -90f;
    private const float OffsetLocalY = 320f;

    // Глобальный счетчик верхнего элемента
    private int topDataIndex = 0;

    private void Awake()
    {
        stepVector = rawDirection.normalized * stepLength;
    }

    private void Start()
    {
        transform.position = new Vector3(0, startPosY, 150);
        targetPos = transform.position;

        InitSlots();
        if (shopButton != null) shopButton.RefreshState();
    }

    private void InitSlots()
    {
        if (allSkinNames == null || allSkinNames.Count == 0 || skinSlots == null) return;

        // Инициализируем стартовые 4 скина
        for (int i = 0; i < skinSlots.Length; i++)
        {
            int circularIndex = GetCircularIndex(i - 1);
            skinSlots[i].SetupSkin(circularIndex, allSkinNames[circularIndex]);
        }
        topDataIndex = skinSlots.Length - 2;
    }
    
    // Листаем вперед
    public void MoveNext()
    {
        if (isMoving) return;

        ConveyorSkinSlot currentSlot = GetSelectedSlot();
        
        if (currentSlot != null && currentSlot.CurrentDataIndex >= allSkinNames.Count - 1)
        {
            Debug.Log("[ConveyorMover] Достигнут конец списка скинов!");
            return;
        }
        
        targetPos += stepVector;
        StartMove(isNext: true);
    }

    // Листаем назад
    public void MoveBack()
    {
        if (isMoving) return;
        
        Vector3 nextPos = targetPos - stepVector;
        if (nextPos.y > startPosY || nextPos.z > 150) return;
        
        targetPos -= stepVector;
        StartMove(isNext: false);
    }

    private void StartMove(bool isNext)
    {
        if (ConveyorSFX_source != null && ConveyorSFX != null)
        {
            ConveyorSFX_source.PlayOneShot(ConveyorSFX);
        }

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveRoutine(isNext));
    }

    private IEnumerator MoveRoutine(bool isNext)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPos, 
                speed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = targetPos;
        
        WrapPoolObjects(isNext);

        isMoving = false;
        
        if (shopButton != null) shopButton.RefreshState();
    }

    private void WrapPoolObjects(bool isNext)
    {
        if (skinSlots == null || skinSlots.Length == 0 || allSkinNames.Count == 0) return;
        
        ConveyorSkinSlot lowestSlot = skinSlots[0];
        ConveyorSkinSlot highestSlot = skinSlots[0];

        for (int i = 1; i < skinSlots.Length; i++)
        {
            if (skinSlots[i].transform.localPosition.y < lowestSlot.transform.localPosition.y)
                lowestSlot = skinSlots[i];

            if (skinSlots[i].transform.localPosition.y > highestSlot.transform.localPosition.y)
                highestSlot = skinSlots[i];
        }

        if (isNext)
        {
            // Листаем вперед: нижний слот летит наверх
            if (lowestSlot.transform.position.y < ThresholdGlobalY)
            {
                Vector3 pos = lowestSlot.transform.localPosition;
                pos.y += OffsetLocalY;
                lowestSlot.transform.localPosition = pos;

                // Смещаем индекс вперед
                topDataIndex++;
                int circularIdx = GetCircularIndex(topDataIndex);
                lowestSlot.SetupSkin(circularIdx, allSkinNames[circularIdx]);
            }
        }
        else
        {
            // Листаем назад: верхний слот летит вниз
            Vector3 pos = highestSlot.transform.localPosition;
            pos.y -= OffsetLocalY;
            highestSlot.transform.localPosition = pos;

            // Вычисляем индекс скина снизу
            int bottomIndex = topDataIndex - skinSlots.Length;
            int circularIdx = GetCircularIndex(bottomIndex);
            highestSlot.SetupSkin(circularIdx, allSkinNames[circularIdx]);
            
            topDataIndex--;
        }
    }

    public ConveyorSkinSlot GetSelectedSlot()
    {
        if (skinSlots == null || skinSlots.Length == 0) return null;
        // Сортируем слоты по высоте
        var sortedSlots = skinSlots.OrderBy(s => s.transform.localPosition.y).ToArray();
        
        return sortedSlots[1];
    }

    
    /// <summary>
    /// Математический остаток от деления, поддерживающий отрицательные числа
    /// </summary>
    private int GetCircularIndex(int index)
    {
        int count = allSkinNames.Count;
        return ((index % count) + count) % count;
    }
}
