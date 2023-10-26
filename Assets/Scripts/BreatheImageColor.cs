using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class BreatheImageColor : MonoBehaviour
{
    //Fade an image between two colors with an infinite tween by startup.

    [Header("References")]
    [SerializeField] Image image;

    [Header("Config")]
    [SerializeField] Color colorOne;
    [SerializeField] Color colorTwo;
    [SerializeField] float breathTime;

    void Start()
    {
        image.DOColor(colorTwo, breathTime).From(colorOne).SetLoops(-1, LoopType.Yoyo);
    }
}
