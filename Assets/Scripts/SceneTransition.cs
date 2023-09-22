using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{

    public GameObject screen;
    Image scr;
    public CanvasGroup menu;
    public float duration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        screen.SetActive(true);
        scr = screen.GetComponent<Image>();
        menu.interactable = false;
        scr.CrossFadeAlpha(0, duration, false);
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(duration);
        menu.interactable = true;
        //Debug.Log("interactable");
        screen.SetActive(false);
    }
}
