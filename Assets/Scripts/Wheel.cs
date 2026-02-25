using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class Wheel : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] private List<Cell> cells;
    [SerializeField] private Pointer pointer;
    
    [Header("Wheel Rotation Settings")]
    [SerializeField] private float rotationDuration = 10f;
    [SerializeField] private float pauseBeforeReturn = 1.5f;
    [SerializeField] private Ease rotationEase = Ease.OutCubic;
    [SerializeField] private int fullRotations = 2;
    [SerializeField] private float rotationvelocity;
    
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
    private float targetRotationAngle;
    private Coroutine returnCoroutine;
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
        
        
        int targetIndex = cells.IndexOf(selectedCell);
        
        
        float cellAngle = startAngle + (targetIndex * anglePerCell);
        
        
        float currentY = transform.eulerAngles.y;
        float targetY = NormalizeAngle(cellAngle);
        
       
        float angleToTarget = (targetY - currentY + 360f) % 360f;
        
        
        if (angleToTarget < 30f)
        {
            angleToTarget += 360f;
        }
        
        
        float totalRotation = angleToTarget + (360f * fullRotations);
        
        
        targetRotationAngle = currentY + totalRotation;
        
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
    
    private void OnSpinComplete()
    {
        
        returnCoroutine =StartCoroutine(ReturnToStartPosition());
    }
    
    private IEnumerator ReturnToStartPosition()
    {
        
        yield return new WaitForSeconds(pauseBeforeReturn);
        rotationTween = transform.DORotate(
            new Vector3(90, startAngle, 0),
            rotationDuration * 0.5f, 
            RotateMode.FastBeyond360
        )
        .SetEase(rotationEase)
        .OnComplete(() => {
            isSpinning = false;
            returnCoroutine = null;
           
            OnCellLanded?.Invoke(selectedCell);
            
            
        });
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanSpin) return;
        lastpos_mouse=eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isSpinning && !CanSpin) return;
        
        float deltaX = eventData.position.x - lastpos_mouse.x;
        rotationAmount = deltaX * rotationSpeed*-1;
        transform.Rotate(0, rotationAmount, 0, Space.World);
        lastpos_mouse = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SpinToRandomCell();
        CanSpin = false;
    }
    public bool IsSpinning => isSpinning;
    
    private void OnDestroy()
    {
        if (rotationTween != null && rotationTween.IsActive())
            rotationTween.Kill();
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }
    }
}