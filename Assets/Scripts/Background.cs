using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Background : MonoBehaviour
{
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }
	
    public void SetBackground(Sprite newImage)
    {
        image.sprite = newImage;
    }
}
