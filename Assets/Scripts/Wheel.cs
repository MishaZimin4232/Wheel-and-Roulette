using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class Wheel : MonoBehaviour
{
    [SerializeField] private List<Cell> cells ;
    [SerializeField] private Pointer pointer;
    
    [Header("Wheel Rotation Settings")]
    [SerializeField] private float rotationDuration = 10f;
    [SerializeField] private Ease rotationEase = Ease.OutCubic;
    [SerializeField] private int fullRotations = 2;
    

    public event Action<Cell> OnCellLanded;
    
    private bool isSpinning = false;
    private Tween rotationTween;
    

    private Cell selectedCell;
    
    private void Awake()
    {
        if (pointer == null)
            pointer = FindObjectOfType<Pointer>();
        Cell[] foundCells = FindObjectsByType<Cell>(FindObjectsSortMode.None);
        cells = new List<Cell>(foundCells);   
       
    }
    
    
    

    public void SpinToRandomCell()
    {
        if (isSpinning)
        {
            Debug.Log("Колесо уже вращается!");
            return;
        }
        
       
        SelectRandomCell();
        
      
        StartSpinAnimation();
    }
    

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
        
      
        selectedCell = targetCell;
        
  
        StartSpinAnimation();
    }
    
  
    private void SelectRandomCell()
    {
        if (cells == null || cells.Count == 0)
        {
            Debug.LogError("Нет клеток на колесе!");
            return;
        }
        
        selectedCell = cells[UnityEngine.Random.Range(0, cells.Count)];
       
    }
    

    private void StartSpinAnimation()
    {
        if (selectedCell == null)
        {
            Debug.LogError("Не выбрана клетка для вращения!");
            return;
        }
        
        isSpinning = true;
        
        
        float targetAngle = CalculateAngleToSelectedCell();
        float currentAngle = transform.eulerAngles.y;
        float finalAngle = currentAngle + (360f * fullRotations) + targetAngle;
        
        
        
       
        rotationTween = transform.DORotate(
            new Vector3(transform.eulerAngles.x, finalAngle, transform.eulerAngles.z),
            rotationDuration,
            RotateMode.FastBeyond360
        )
        .SetEase(rotationEase)
        .OnComplete(OnSpinComplete);
    }
    
    
    private float CalculateAngleToSelectedCell()
    {
        int targetIndex = cells.IndexOf(selectedCell);
        if (targetIndex == -1) return 0f;
        
        float anglePerCell = 360f / cells.Count;
        float cellCenterAngle = targetIndex * anglePerCell + (anglePerCell / 2f);
        float pointerAngle = pointer != null ? pointer.transform.eulerAngles.y : 0f;
        
        return (cellCenterAngle - pointerAngle + 360f) % 360f;
    }
    
    
    private void OnSpinComplete()
    {
        isSpinning = false;
        
        // Точная доводка
        float exactAngle = CalculateAngleToSelectedCell();
        transform.DORotate(new Vector3(transform.eulerAngles.x, exactAngle, transform.eulerAngles.z), 0.1f);
        
        
        
        // ВАЖНО: Вызываем событие с ВЫБРАННОЙ клеткой
        OnCellLanded?.Invoke(selectedCell);
    }
    
    
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