using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSwapUI : MonoBehaviour, GenericActivator
{
    [SerializeField] GameObject toSwap;
    [SerializeField] List<Material> materials;

    private Image img;

    void Awake()
    {
        img = toSwap.GetComponent<Image>();
    }

    public void activate(int activationStyle)
    {
        img.material = materials[activationStyle];
    }

    public void deactivate(int activationStyle)
    {
        // Do Nothing
    }
}
