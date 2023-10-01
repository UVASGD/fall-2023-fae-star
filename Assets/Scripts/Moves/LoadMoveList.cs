using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMoveList : MonoBehaviour
{
    [SerializeField] List<GameObject> actionLists;
    [SerializeField] GameObject actionSelectItemPrefab;
    [SerializeField] RectTransform backingCostFillImage;
    [SerializeField] RectTransform fillImage;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < actionLists.Count; i++)
        {
            GameObject actionList = actionLists[i];
            Selection selection = actionList.transform.GetChild(0).gameObject.GetComponent<Selection>();
            Debug.Log(selection);
            int size = GlobalMoveLists.MoveList[i].Count;
            RectTransform scrollView = (RectTransform) actionList.transform.GetChild(1);
            RectTransform content = (RectTransform) actionList.transform.GetChild(1).GetChild(0).GetChild(0);
            RectTransform actionSelectTransform = actionSelectItemPrefab.GetComponent<RectTransform>();
            content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Max(scrollView.sizeDelta.y, 30 + actionSelectTransform.sizeDelta.y * size));
            for (int j = 0; j < size; j++)
            {
                GameObject currentItem = Instantiate(actionSelectItemPrefab, content);
                DisplayActionCost displayActionCost = currentItem.GetComponent<DisplayActionCost>();
                displayActionCost.backingCostFillImage = backingCostFillImage;
                displayActionCost.fillImage = fillImage;
                selection.AddSwapActivators(displayActionCost);
            }
        }
    }
}
