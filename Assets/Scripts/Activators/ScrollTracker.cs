using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTracker : MonoBehaviour, GenericActivator
{
    [SerializeField] Scrollbar scrollbar;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void activate(int activationStyle)
    {
        // Your code will likely have some sort of  manipulation of this value based off of the position of the selected object
        scrollbar.value = scrollbar.value - 0.1f;
    }

    public void deactivate(int activationStyle)
    {
        // Do Nothing
    }
}
