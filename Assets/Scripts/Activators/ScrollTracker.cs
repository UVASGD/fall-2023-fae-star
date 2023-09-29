using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScrollTracker : MonoBehaviour, GenericActivator
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] RectTransform scrollView;
    [SerializeField] RectTransform contentHolder;
    [SerializeField] RectTransform outline;

    public void activate(int activationStyle, int swapSource)
    {
        if (swapSource == 0)
        {
            float outlineBottom = outline.parent.localPosition.y + contentHolder.localPosition.y  - outline.sizeDelta.y;
            float outlineTop = outline.parent.localPosition.y + contentHolder.localPosition.y;
            if (outlineBottom < -scrollView.sizeDelta.y + 15)
            {
                int bottomDistValue = (int) (-outlineBottom - scrollView.sizeDelta.y);
                scrollbar.value -= (float) bottomDistValue / (contentHolder.sizeDelta.y - scrollView.sizeDelta.y);
            }
            else if(outlineTop > -15)
            {
                int topDistValue = (int) (outlineTop + 15);
                scrollbar.value += (float) topDistValue / (contentHolder.sizeDelta.y - scrollView.sizeDelta.y);
            }
        }
    }
}
