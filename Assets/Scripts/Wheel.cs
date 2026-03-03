using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class Wheel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<Cell> cells;
    [SerializeField] private Pointer pointer;
    
    [Header("Wheel Rotation Settings")]
    [SerializeField] private float rotationDuration = 10f;
    [SerializeField] private Ease rotationEase = Ease.OutCubic;
    [SerializeField] private int fullRotations = 5;
    
    [Header("Wheel Setup")]
    [SerializeField] private float startAngle = -160f;
    [SerializeField] private float anglePerCell = 10f;
    private Vector2 lastpos_mouse;
    private float rotationAmount;
    private float rotationSpeed = 0.5f;
    
    public event Action<Cell> OnCellLanded;
    
    private bool isSpinning = false;
    private Tween rotationTween;
    private Cell selectedCell;
    public bool CanSpin;
    
    private void Awake()
    {
        if (pointer == null)
            pointer = FindObjectOfType<Pointer>();
        SetWheelToStartPosition();
        CanSpin = false;
    }
    
    private void SetWheelToStartPosition()
    {
        transform.rotation = Quaternion.Euler(90, startAngle, 0);
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
        
        
        float currentY = transform.eulerAngles.y;
        
        
        int targetIndex = cells.IndexOf(selectedCell);
        float targetCellAngle = NormalizeAngle(startAngle + (targetIndex * anglePerCell));
        
        
        float angleDifference = CalculateShortestAngle(currentY, targetCellAngle);
        
       
        if (Mathf.Abs(angleDifference) < 30f)
        {
            angleDifference += 360f;
        }
        
       
        float totalRotation = angleDifference + (360f * fullRotations);
        
        
        float targetRotationAngle = currentY + totalRotation;
        
        
        rotationTween = transform.DORotate(
            new Vector3(90, targetRotationAngle, 0),
            rotationDuration,
            RotateMode.FastBeyond360
        )
        .SetEase(rotationEase)
        .OnComplete(OnSpinComplete);
    }
    
    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }
    
    private float CalculateShortestAngle(float fromAngle, float toAngle)
    {
        float difference = (toAngle - fromAngle + 540f) % 360f - 180f;
        return difference;
    }
    
    private void OnSpinComplete()
    {
        isSpinning = false;
        
        Vector3 currentRot = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(90, NormalizeAngle(currentRot.y), 0);
        
        OnCellLanded?.Invoke(selectedCell);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanSpin || isSpinning) return;
        lastpos_mouse = eventData.position;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!CanSpin || isSpinning) return;
        
        float deltaX = eventData.position.x - lastpos_mouse.x;
        rotationAmount = deltaX * rotationSpeed * -1;
        transform.Rotate(0, rotationAmount, 0, Space.World);
        lastpos_mouse = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanSpin) return;
        
        
        FindClosestCellToPointer();
        StartSpinAnimation();
        CanSpin = false;
    }
    
    private void FindClosestCellToPointer()
    {
        if (pointer == null || cells == null || cells.Count == 0) return;
        
        float minAngleDifference = float.MaxValue;
        Cell closestCell = null;
        
        float currentWheelY = transform.eulerAngles.y;
        
        foreach (Cell cell in cells)
        {
            int cellIndex = cells.IndexOf(cell);
            float cellAngle = NormalizeAngle(startAngle + (cellIndex * anglePerCell));
            float angleDifference = Mathf.Abs(CalculateShortestAngle(currentWheelY, cellAngle));
            
            if (angleDifference < minAngleDifference)
            {
                minAngleDifference = angleDifference;
                closestCell = cell;
            }
        }
        
        if (closestCell != null)
        {
            selectedCell = closestCell;
        }
    }
    
    public bool IsSpinning => isSpinning;
    
    private void OnDestroy()
    {
        if (rotationTween != null && rotationTween.IsActive())
            rotationTween.Kill();
    }
}