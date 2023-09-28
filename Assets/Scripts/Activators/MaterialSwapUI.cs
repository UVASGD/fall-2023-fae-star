using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSwapUI : MonoBehaviour, IActivator
{
    [SerializeField] GameObject toSwap;
    [SerializeField] List<Material> materials;

    private Image img;

    void Awake()
    {
        img = toSwap.GetComponent<Image>();
    }

    public void Activate(int activationStyle)
    {
        img.material = materials[activationStyle];
    }

    public void Deactivate(int activationStyle)
    {
        // Do Nothing
    }
}
