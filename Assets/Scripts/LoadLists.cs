using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LoadLists : MonoBehaviour
{
    [SerializeField] List<GameObject> actionLists;
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject actionSelectItemPrefab;
    [SerializeField] GameObject itemSelectItemPrefab;
    [SerializeField] RectTransform backingCostFillImage;
    [SerializeField] RectTransform fillImage;

    // Start is called before the first frame update
    public void Awake()
    {
        Selection itemSelection = itemList.transform.GetChild(0).gameObject.GetComponent<Selection>();
        int itemSize = GlobalItemLists.ItemList.Count;

        // Adjust the sizes of the content box
        RectTransform itemScrollView = (RectTransform)itemList.transform.GetChild(1);
        RectTransform itemContent = (RectTransform)itemList.transform.GetChild(1).GetChild(0).GetChild(0);
        RectTransform itemSelectTransform = itemSelectItemPrefab.GetComponent<RectTransform>();
        itemContent.sizeDelta = new Vector2(itemContent.sizeDelta.x, Mathf.Max(itemScrollView.sizeDelta.y, 30 + itemSelectTransform.sizeDelta.y * itemSize));

        // Loop through the Dictionary
        int itemAt = 0;
        foreach (KeyValuePair<string, (Item, int)> kvp in GlobalItemLists.ItemList)
        {
            // Instantiate object with parent as the transform of the content box
            GameObject currentItem = Instantiate(itemSelectItemPrefab, itemContent);

            // Set up the values of each action item in the list
            currentItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = kvp.Key;
            int amount = 0;
            if (kvp.Value.Item1 != null)
            {
                amount = kvp.Value.Item1.getAmount();
                currentItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amount + "";
            }
            else
            {
                currentItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            currentItem.transform.localPosition = new Vector3(15, -15 - itemSelectTransform.sizeDelta.y * itemAt, 0);

            // Setting up the event trigger stuff
            EventTrigger trigger = currentItem.GetComponent<EventTrigger>();
            EventTrigger.Entry hoverTrigger = new EventTrigger.Entry();
            hoverTrigger.eventID = EventTriggerType.PointerEnter;
            hoverTrigger.callback.AddListener((eventData) => { itemSelection.SetSelect(kvp.Value.Item2); });
            trigger.triggers.Add(hoverTrigger);
            EventTrigger.Entry clickTrigger = new EventTrigger.Entry();
            clickTrigger.eventID = EventTriggerType.PointerClick;
            clickTrigger.callback.AddListener((eventData) => { itemSelection.Select(); });
            trigger.triggers.Add(clickTrigger);

            // Complete external list connections
            DisplayActionCost displayActionCost = currentItem.GetComponent<DisplayActionCost>();
            displayActionCost.actionListIndex = itemAt;
            displayActionCost.cost = amount;
            displayActionCost.backingCostFillImage = backingCostFillImage;
            displayActionCost.fillImage = fillImage;
            itemSelection.AddSwapActivators(displayActionCost);
            itemSelection.items.Add(currentItem);
            itemSelection.listSizes.Add(1);

            // Increment the tracking variable
            itemAt++;
        }
        for(int i = 0; i < actionLists.Count; i++)
        {
            GameObject actionList = actionLists[i];
            Selection selection = actionList.transform.GetChild(0).gameObject.GetComponent<Selection>();
            int size = GlobalMoveLists.MoveList[i].Count;

            // Adjust the sizes of the content box
            RectTransform scrollView = (RectTransform) actionList.transform.GetChild(1);
            RectTransform content = (RectTransform) actionList.transform.GetChild(1).GetChild(0).GetChild(0);
            RectTransform actionSelectTransform = actionSelectItemPrefab.GetComponent<RectTransform>();
            content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Max(scrollView.sizeDelta.y, 30 + actionSelectTransform.sizeDelta.y * size));

            // Loop through the Dictionary
            int at = 0;
            foreach (KeyValuePair<string, (Move.ActionTypes, int, int)> kvp in GlobalMoveLists.MoveList[i])
            {
                // Instantiate object with parent as the transform of the content box
                GameObject currentItem = Instantiate(actionSelectItemPrefab, content);

                // Set up the values of each action item in the list
                currentItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = kvp.Key;
                int manaCost = 0;
                if (kvp.Key != null)
                {
                    manaCost = kvp.Value.Item2;
                    currentItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (manaCost + "mp");
                }
                else
                {
                    currentItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                currentItem.transform.localPosition = new Vector3(15, -15 - actionSelectTransform.sizeDelta.y * at, 0);
                
                // Setting up the event trigger stuff
                EventTrigger trigger = currentItem.GetComponent<EventTrigger>();
                EventTrigger.Entry hoverTrigger = new EventTrigger.Entry();
                hoverTrigger.eventID = EventTriggerType.PointerEnter;
                hoverTrigger.callback.AddListener((eventData) => { selection.SetSelect(kvp.Value.Item3); });
                trigger.triggers.Add(hoverTrigger);
                EventTrigger.Entry clickTrigger = new EventTrigger.Entry();
                clickTrigger.eventID = EventTriggerType.PointerClick;
                clickTrigger.callback.AddListener((eventData) => { selection.Select(); });
                trigger.triggers.Add(clickTrigger);

                // Complete external list connections
                DisplayActionCost displayActionCost = currentItem.GetComponent<DisplayActionCost>();
                displayActionCost.actionListIndex = at;
                displayActionCost.cost = manaCost;
                displayActionCost.backingCostFillImage = backingCostFillImage;
                displayActionCost.fillImage = fillImage;
                selection.AddSwapActivators(displayActionCost);
                selection.items.Add(currentItem);
                selection.listSizes.Add(1);

                // Increment the tracking variable
                at++;
            }
        }

        Destroy(gameObject);
    }
}
