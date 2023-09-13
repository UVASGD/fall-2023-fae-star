using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxBarSpriteSwap : MonoBehaviour
{
    public Sprite primaryImage;
    public Sprite secondaryImage;
    [SerializeField] private GameObject observeredObject;

    private Slider observeredSlider;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        observeredSlider = observeredObject.GetComponent<Slider>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(image.sprite != secondaryImage)
        {
            if (observeredSlider.value >= observeredSlider.maxValue)
            {
                image.sprite = secondaryImage;
            }
        }
        else
        {
            if(observeredSlider.value < observeredSlider.maxValue)
            {
                image.sprite = primaryImage;
            }
        }
    }
}
