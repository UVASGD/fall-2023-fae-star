using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    [SerializeField] List<GameObject> items;
    [SerializeField] List<int> listSizes;
    [SerializeField] GameObject outliner;
    [SerializeField] GameObject reverseActivation;
    [SerializeField] GameObject[] swapActivators;
    private List<GenericActivator> iSwapActivators;

    private List<List<GameObject>> listItems;
    private int x = 0;
    private int y = 0;
    private RectTransform rtf;

    void Awake()
    {
        rtf = outliner.GetComponent<RectTransform>();

        iSwapActivators = new List<GenericActivator>();
        for (int i = 0; i < swapActivators.Length; i++)
        {
            iSwapActivators.Add(swapActivators[i].GetComponent(typeof(GenericActivator)) as GenericActivator);
        }

        // List instantiation bc Unity is stupid and doesn't allow serialization of 2D arrays
        listItems = new List<List<GameObject>>();
        for(int i = 0; i < listSizes.Count; i++)
        {
            listItems.Add(new List<GameObject>());
        }

        int xCoord = 0;
        int at = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if(at == listSizes[xCoord])
            {
                xCoord++;
                at = 0;
            }
            listItems[xCoord].Add(items[i]);
            at++;
        }

        adjustOutline();
    }

    public void ResetSelection()
    {
        x = 0;
        y = 0;
        if (listItems != null)
        {
            onSwap(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            x--;
            x += listItems.Count;
            x %= listItems.Count;
            y = Mathf.Min(y, listItems[x].Count);
            onSwap(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            x++;
            x %= listItems.Count;
            y = Mathf.Min(y, listItems[x].Count);
            onSwap(0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            y--;
            y += listItems[x].Count;
            y %= listItems[x].Count;
            onSwap(0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            y++;
            y %= listItems[x].Count;
            onSwap(0);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Select();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReverseSelect();
        }
    }

    // Activates all activators related to swapping to the current option
    void onSwap(int swapSource) //swapSource 0 means keyboard, swapSource 1 means mouse
    {
        adjustOutline();

        if (iSwapActivators.Count != 0)
        {
            int currentSelection = currentlySelected();

            foreach (GenericActivator activator in iSwapActivators)
            {
                activator.activate(currentSelection, swapSource);
            }
        }
    }

    // Readjusts the outline to surround currently selected object
    void adjustOutline()
    {
        RectTransform parentTransform = (RectTransform) listItems[x][y].transform;
        rtf.SetParent(parentTransform);
        rtf.localPosition = new Vector3((0.5f - parentTransform.anchorMin.x) * parentTransform.sizeDelta.x, (0.5f - parentTransform.anchorMin.y) * parentTransform.sizeDelta.y, parentTransform.position.z);
        Vector2 newSize = new Vector2(parentTransform.sizeDelta.x + (2f / 15f * 100f), parentTransform.sizeDelta.y + (2f / 15f * 100f));
        rtf.sizeDelta = newSize;
    }

    // Allows for external signals to swap the selection, treats objects as if they were non-zero indexed (because hackey solution)
    public void SetSelect(int xy)
    {
        x = xy / 10 - 1;
        y = xy % 10 - 1;
        onSwap(1);
    }

    // Activates when player confirms selection
    public void Select()
    {
        int selected = currentlySelected();
        gameObject.SetActive(false);
        TransitionManager.processTransition(selected);
    }

    public void ReverseSelect()
    {
        if (reverseActivation != null)
        {
            reverseActivation.SetActive(true);
            TransitionManager.reverseTransition();
        }
    }


    // Helper method to determine current selection
    private int currentlySelected()
    {
        int currentSelection = 0;
        for (int i = 0; i < x; i++)
        {
            currentSelection += listItems[i].Count;
        }
        currentSelection += y;
        return currentSelection;
    }
}
