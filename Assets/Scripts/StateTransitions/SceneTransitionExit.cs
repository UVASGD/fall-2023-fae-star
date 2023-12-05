using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionExit : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] GameObject otherScreen;
    [SerializeField] CanvasGroup menu;
    [SerializeField] float duration = 2f;

    private DataPersistenceManager dataPersistenceManager;
    private Image scr;
    private bool finished;

    // Start is called before the first frame update
    void Awake()
    {
        screen.SetActive(true);
        scr = screen.GetComponent<Image>();
        menu.interactable = false;
        scr.CrossFadeAlpha(1, duration, false);
        StartCoroutine(CooldownOut());
    }

    IEnumerator CooldownOut()
    {
        yield return new WaitForSeconds(duration + 1);
        //Debug.Log("interactable");
        finished = true;
    }
    
    void Update()
    {
        if(finished)
        {
            SceneManager.LoadScene("DialogueScene");
        }
    }
}
