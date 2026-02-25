using UnityEngine;

public class Pointer : MonoBehaviour
{
    
    private Cell currentCell;
    
    
    
    
    void Start()
    {
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        Cell detectedCell = other.GetComponent<Cell>();
        if (detectedCell != null)
        {
            currentCell = detectedCell;
            
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        
        Cell detectedCell = other.GetComponent<Cell>();
        if (detectedCell != null && currentCell != detectedCell)
        {
            currentCell = detectedCell;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Cell exitedCell = other.GetComponent<Cell>();
        if (exitedCell != null && currentCell == exitedCell)
        {
            
            currentCell = null;
        }
    }
}