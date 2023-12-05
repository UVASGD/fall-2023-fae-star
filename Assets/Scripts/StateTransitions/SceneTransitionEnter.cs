using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionEnter : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] CanvasGroup menu;
    [SerializeField] float duration = 2f;
    private Image scr;

    // Start is called before the first frame update
    void Awake()
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
        GlobalStateTracker.battleState = GlobalStateTracker.States.CharacterSelect;
        //Debug.Log("interactable");
        screen.SetActive(false);
    }
}
