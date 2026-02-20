using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class Wheel : MonoBehaviour
{
    [SerializeField] private List<Cell> cells = new List<Cell>();
    [SerializeField] private Pointer pointer;
    
    [Header("Wheel Rotation Settings")]
    [SerializeField] private float rotationDuration = 10f;
    [SerializeField] private Ease rotationEase = Ease.OutCubic;
    [SerializeField] private int fullRotations = 2;
    
    // Событие: передаём клетку, которая БЫЛА ВЫБРАНА и теперь остановилась
    public event Action<Cell> OnCellLanded;
    
    private bool isSpinning = false;
    private Tween rotationTween;
    
    // Клетка, которая БЫЛА ВЫБРАНА до вращения
    private Cell selectedCell;
    
    private void Awake()
    {
        if (pointer == null)
            pointer = FindObjectOfType<Pointer>();
            
        ValidateCells();
    }
    
    private void ValidateCells()
    {
        if (cells == null || cells.Count == 0)
        {
            cells = new List<Cell>();
            cells.AddRange(GetComponentsInChildren<Cell>());
            Debug.Log($"Найдено клеток: {cells.Count}");
        }
    }
    
    /// <summary>
    /// ВЫБИРАЕТ случайную клетку и начинает вращение к ней
    /// </summary>
    public void SpinToRandomCell()
    {
        if (isSpinning)
        {
            Debug.Log("Колесо уже вращается!");
            return;
        }
        
        // 1. ВЫБИРАЕМ случайную клетку (ДО вращения)
        SelectRandomCell();
        
        // 2. Запускаем вращение к выбранной клетке
        StartSpinAnimation();
    }
    
    /// <summary>
    /// ВЫБИРАЕТ конкретную клетку и вращается к ней
    /// </summary>
    public void SpinToCell(Cell targetCell)
    {
        if (isSpinning)
        {
            
            return;
        }
        
        if (targetCell == null || !cells.Contains(targetCell))
        {
            Debug.LogError("Целевая клетка не найдена на колесе!");
            return;
        }
        
        // 1. ВЫБИРАЕМ конкретную клетку
        selectedCell = targetCell;
        
        // 2. Запускаем вращение
        StartSpinAnimation();
    }
    
    /// <summary>
    /// Выбрать случайную клетку (ДО вращения)
    /// </summary>
    private void SelectRandomCell()
    {
        if (cells == null || cells.Count == 0)
        {
            Debug.LogError("Нет клеток на колесе!");
            return;
        }
        
        selectedCell = cells[UnityEngine.Random.Range(0, cells.Count)];
       
    }
    
    /// <summary>
    /// Запустить анимацию вращения к выбранной клетке
    /// </summary>
    private void StartSpinAnimation()
    {
        if (selectedCell == null)
        {
            Debug.LogError("Не выбрана клетка для вращения!");
            return;
        }
        
        isSpinning = true;
        
        // Вычисляем угол до выбранной клетки
        float targetAngle = CalculateAngleToSelectedCell();
        float currentAngle = transform.eulerAngles.y;
        float finalAngle = currentAngle + (360f * fullRotations) + targetAngle;
        
        
        
        // Запускаем анимацию
        rotationTween = transform.DORotate(
            new Vector3(transform.eulerAngles.x, finalAngle, transform.eulerAngles.z),
            rotationDuration,
            RotateMode.FastBeyond360
        )
        .SetEase(rotationEase)
        .OnComplete(OnSpinComplete);
    }
    
    /// <summary>
    /// Вычислить угол до выбранной клетки
    /// </summary>
    private float CalculateAngleToSelectedCell()
    {
        int targetIndex = cells.IndexOf(selectedCell);
        if (targetIndex == -1) return 0f;
        
        float anglePerCell = 360f / cells.Count;
        float cellCenterAngle = targetIndex * anglePerCell + (anglePerCell / 2f);
        float pointerAngle = pointer != null ? pointer.transform.eulerAngles.y : 0f;
        
        return (cellCenterAngle - pointerAngle + 360f) % 360f;
    }
    
    /// <summary>
    /// Вызывается когда вращение завершено
    /// </summary>
    private void OnSpinComplete()
    {
        isSpinning = false;
        
        // Точная доводка
        float exactAngle = CalculateAngleToSelectedCell();
        transform.DORotate(new Vector3(transform.eulerAngles.x, exactAngle, transform.eulerAngles.z), 0.1f);
        
        
        
        // ВАЖНО: Вызываем событие с ВЫБРАННОЙ клеткой
        OnCellLanded?.Invoke(selectedCell);
    }
    
    /// <summary>
    /// Получить текущую выбранную клетку (без вращения)
    /// </summary>
    public Cell GetSelectedCell()
    {
        return selectedCell;
    }
    
    public bool IsSpinning => isSpinning;
    
    private void OnDestroy()
    {
        if (rotationTween != null && rotationTween.IsActive())
            rotationTween.Kill();
    }
}