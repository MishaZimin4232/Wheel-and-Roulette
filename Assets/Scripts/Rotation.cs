using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotation : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    
    [SerializeField] private float rotationSpeed = 0.5f;
    private Vector2 lastMousePosition;
    private Rigidbody rb;
    private float rotationAmount;

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        lastMousePosition = eventData.position;
        rb.angularVelocity=Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaX = eventData.position.x - lastMousePosition.x;
        rotationAmount = deltaX * rotationSpeed * -1;
        transform.Rotate(0f, rotationAmount, 0f, Space.World);
        lastMousePosition = eventData.position;
        
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
       
    }
    

}