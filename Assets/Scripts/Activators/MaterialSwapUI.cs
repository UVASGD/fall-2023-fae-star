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

    public void activate(int activationStyle, int source)
    {
        img.material = materials[activationStyle];
    }
}
