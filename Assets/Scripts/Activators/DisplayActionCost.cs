using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayActionCost : MonoBehaviour, IActivator
{
    //Display the cost of an action by hiding the portion of the respective bar to show the change before and after
    //TODO : Make this script reference the active character's stats to change the bar's width in accordance with current and max mana/health
    [Header("References")]
    [SerializeField] public int actionListIndex; //What index in the action list it is. Definitely a way to automatically set this
    [SerializeField] public int cost;
    [SerializeField] public RectTransform backingCostFillImage;
    [SerializeField] public RectTransform fillImage;

    float defaultFillImageWidth;
    public bool active = false;

    void Start()
    {
        //Keep track of the actual width of the bar for when we switch off / choose the action
        defaultFillImageWidth = fillImage.sizeDelta.x;
    }

    public void Activate(int activationStyle, int source)
    {
        if(activationStyle != actionListIndex) return;
        active = true;

        //Make the flashing "cost" bar keep the width of the original
        backingCostFillImage.sizeDelta = new Vector2(defaultFillImageWidth, fillImage.sizeDelta.y);

        //Make the actual bar go down to the width after using the proposed action
        //TODO : replace this lerp with the character's actual maximum mana/health
        //TODO : replace this lerp with the action's actual mana/health cost
        fillImage.sizeDelta = new Vector2(Mathf.Lerp(defaultFillImageWidth, 0, (float)cost / 10), fillImage.sizeDelta.y);
    }

    public void Deactivate(int deactivationStyle, int source)
    {
        if(!active) return;

        //Put the bar's width back to what it's supposed to be
        fillImage.sizeDelta = new Vector2(defaultFillImageWidth, fillImage.sizeDelta.y);

        //Hide the flashing bar representing the cost of the action
        backingCostFillImage.sizeDelta = new Vector2(defaultFillImageWidth, backingCostFillImage.sizeDelta.y);

        active = false;
    }

    public void Reset()
    {
        //Put the bar's width back to what it's supposed to be
        fillImage.sizeDelta = new Vector2(defaultFillImageWidth, fillImage.sizeDelta.y);

        //Hide the flashing bar representing the cost of the action
        backingCostFillImage.sizeDelta = new Vector2(0, backingCostFillImage.sizeDelta.y);
    }
}
