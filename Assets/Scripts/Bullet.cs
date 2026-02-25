using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    
    private Image myImage;

    
    [SerializeField]private Sprite sprite1;
    [SerializeField]private Sprite sprite2;
    void Start()
    {
       
        myImage = GetComponent<Image>();
    }

    public void ChangeSprite()
    {
        if (myImage.sprite == sprite2)
            myImage.sprite = sprite1;
        else
            myImage.sprite = sprite2;
    }
}
