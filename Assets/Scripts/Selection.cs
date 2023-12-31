using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    [SerializeField] public List<GameObject> items;
    [SerializeField] public List<int> listSizes;
    [SerializeField] GameObject outliner;
    [SerializeField] GameObject[] swapActivators;
    private List<IActivator> iSwapActivators = new List<IActivator>();

    private List<List<GameObject>> listItems;
    private List<List<bool>> locks;
    private int x = 0;
    private int y = 0;
    private RectTransform rtf;

    void Awake()
    {
        rtf = outliner.GetComponent<RectTransform>();

        for (int i = 0; i < swapActivators.Length; i++)
        {
            iSwapActivators.Add(swapActivators[i].GetComponent(typeof(IActivator)) as IActivator);
        }

        // List instantiation bc Unity is stupid and doesn't allow serialization of 2D arrays
        listItems = new List<List<GameObject>>();
        for (int i = 0; i < listSizes.Count; i++)
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
        if (locks == null)
            spawnLocks();

        adjustOutline();
    }

    public void AddSwapActivators(IActivator activator)
    {
        iSwapActivators.Add(activator);
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
            while (locks[x][y])
            {
                x--;
                x += listItems.Count;
                x %= listItems.Count;
                y = Mathf.Min(y, listItems[x].Count);
            }
            onSwap(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            x++;
            x %= listItems.Count;
            y = Mathf.Min(y, listItems[x].Count);
            while (locks[x][y])
            {
                x++;
                x %= listItems.Count;
                y = Mathf.Min(y, listItems[x].Count);
            }
            onSwap(0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            y--;
            y += listItems[x].Count;
            y %= listItems[x].Count;
            while (locks[x][y])
            {
                y--;
                y += listItems[x].Count;
                y %= listItems[x].Count;
            }
            onSwap(0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            y++;
            y %= listItems[x].Count;
            while (locks[x][y])
            {
                y++;
                y %= listItems[x].Count;
            }
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

            foreach (IActivator activator in iSwapActivators)
            {
                activator.Activate(currentSelection, swapSource);
            }
        }
    }

    // Readjusts the outline to surround currently selected object
    void adjustOutline()
    {
        RectTransform parentTransform = (RectTransform) listItems[x][y].transform;
        rtf.SetParent(parentTransform);
        rtf.SetAsFirstSibling();
        rtf.localPosition = new Vector3((0.5f - parentTransform.anchorMin.x) * parentTransform.sizeDelta.x, (0.5f - parentTransform.anchorMin.y) * parentTransform.sizeDelta.y, parentTransform.position.z);
        Vector2 newSize = new Vector2(parentTransform.sizeDelta.x + (2f / 15f * 100f), parentTransform.sizeDelta.y + (2f / 15f * 100f));
        rtf.sizeDelta = newSize;
    }

    // Allows for external signals to swap the selection, treats objects as if they were non-zero indexed (because hackey solution)
    public void SetSelect(int xy)
    {
        if (gameObject.activeInHierarchy == true)
        {
            SetSelectSecret(xy);
        }
    }

    public void SetSelectSecret(int xy)
    {
        int newx = xy / 10 - 1;
        int newy = xy % 10 - 1;
        if (locks[newx][newy])
            return;
        x = newx;
        y = newy;
        onSwap(1);
    }

    // Activates when player confirms selection
    public void Select()
    {
        if (gameObject.activeInHierarchy == true)
        {
            int selected = currentlySelected();
            gameObject.SetActive(false);
            TransitionManager.processTransition(selected);
        }
    }

    public void ReverseSelect()
    {
        TransitionManager.reverseTransition();
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

    public void setLock(int xy, bool value)
    {
        if (locks == null)
            spawnLocks();
        locks[xy / 10 - 1][xy % 10 - 1] = value;
    }

    public void clearLocks()
    {
        if(locks == null)
        {
            spawnLocks();
            return;
        }
        foreach(List<bool> list in locks)
        {
            for(int i = 0; i < list.Count; i++)
            {
                list[i] = false;
            }
        }
    }

    public bool checkLock(int xy)
    {
        if (locks == null)
            spawnLocks();
        return locks[xy / 10 - 1][xy % 10 - 1];
    }

    public void spawnLocks()
    {
        locks = new List<List<bool>>();
        for (int i = 0; i < listSizes.Count; i++)
        {
            locks.Add(new List<bool>());
        }

        int xCoord = 0;
        int at = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (at == listSizes[xCoord])
            {
                xCoord++;
                at = 0;
            }
            locks[xCoord].Add(false);
            at++;
        }
    }
}
