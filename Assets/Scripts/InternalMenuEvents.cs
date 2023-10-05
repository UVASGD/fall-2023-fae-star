using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InternalMenuEvents : MonoBehaviour, IDragHandler, IScrollHandler
{
    private ScrollRect scrollRect;
    private RectTransform contentTransform;
    
    // Start is called before the first frame update
    void Awake()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
        contentTransform = (RectTransform) (scrollRect.gameObject.transform.GetChild(0).GetChild(0));
    }

    public void OnScroll(PointerEventData pointerEventData)
    {
        scrollRect.verticalScrollbar.value += pointerEventData.scrollDelta.y / (contentTransform.sizeDelta.y / scrollRect.scrollSensitivity / 3);
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        pointerEventData.pointerDrag = scrollRect.gameObject;
        EventSystem.current.SetSelectedGameObject(scrollRect.gameObject);

        scrollRect.OnInitializePotentialDrag(pointerEventData);
        scrollRect.OnBeginDrag(pointerEventData);
    }
}
