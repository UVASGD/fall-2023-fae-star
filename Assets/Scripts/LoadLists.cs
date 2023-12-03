using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LoadMoveAndItemList : MonoBehaviour
{
    [SerializeField] List<GameObject> actionLists;
    [SerializeField] List<GameObject> itemLists;
    [SerializeField] GameObject actionSelectItemPrefab;
    [SerializeField] GameObject itemSelectItemPrefab;
    [SerializeField] RectTransform backingCostFillImage;
    [SerializeField] RectTransform fillImage;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < itemLists.Count; i++)
        {
            GameObject itemList = itemLists[i];
            Selection selection = itemList.transform.GetChild(0).gameObject.GetComponent<Selection>();
            int size = GlobalItemLists.ItemList[i].Count;

            // Adjust the sizes of the content box
            RectTransform scrollView = (RectTransform)itemList.transform.GetChild(1);
            RectTransform content = (RectTransform)itemList.transform.GetChild(1).GetChild(0).GetChild(0);
            RectTransform itemSelectTransform = itemSelectItemPrefab.GetComponent<RectTransform>();
            content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Max(scrollView.sizeDelta.y, 30 + itemSelectTransform.sizeDelta.y * size));

            // Loop through the Dictionary
            int at = 0;
            foreach (KeyValuePair<string, (Item, int)> kvp in GlobalItemLists.ItemList[i])
            {
                // Instantiate object with parent as the transform of the content box
                GameObject currentItem = Instantiate(itemSelectItemPrefab, content);

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
                currentItem.transform.localPosition = new Vector3(15, -15 - itemSelectTransform.sizeDelta.y * at, 0);

                // Setting up the event trigger stuff
                EventTrigger trigger = currentItem.GetComponent<EventTrigger>();
                EventTrigger.Entry hoverTrigger = new EventTrigger.Entry();
                hoverTrigger.eventID = EventTriggerType.PointerEnter;
                hoverTrigger.callback.AddListener((eventData) => { selection.SetSelect(kvp.Value.Item2); });
                trigger.triggers.Add(hoverTrigger);
                EventTrigger.Entry clickTrigger = new EventTrigger.Entry();
                clickTrigger.eventID = EventTriggerType.PointerClick;
                clickTrigger.callback.AddListener((eventData) => { selection.Select(); });
                trigger.triggers.Add(clickTrigger);

                // Complete external list connections
                DisplayActionCost displayActionCost = currentItem.GetComponent<DisplayActionCost>();
                displayActionCost.actionListIndex = at;
                displayActionCost.cost = amount;
                displayActionCost.backingCostFillImage = backingCostFillImage;
                displayActionCost.fillImage = fillImage;
                selection.AddSwapActivators(displayActionCost);
                selection.items.Add(currentItem);
                selection.listSizes.Add(1);

                // Increment the tracking variable
                at++;
            }
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
            foreach (KeyValuePair<string, (Move, int)> kvp in GlobalMoveLists.MoveList[i])
            {
                // Instantiate object with parent as the transform of the content box
                GameObject currentItem = Instantiate(actionSelectItemPrefab, content);

                // Set up the values of each action item in the list
                currentItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = kvp.Key;
                int manaCost = 0;
                if (kvp.Value.Item1 != null)
                {
                    manaCost = kvp.Value.Item1.getManaCost();
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
                hoverTrigger.callback.AddListener((eventData) => { selection.SetSelect(kvp.Value.Item2); });
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
    }
}
