using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    // The name of the initial script
    [SerializeField] string initialScriptName;

    // The object with the internal text of the dialogue box
    [SerializeField] TextMeshProUGUI text;

    // The object with the internal text of the name box
    [SerializeField] TextMeshProUGUI nametag;

    // The piture located on the left side of the dialogue box
    [SerializeField] Image pic1;

    // The picture located on the right side of the dialogue box
    [SerializeField] Image pic2;

    // A full canvas image designated to blocking out the full screen
    [SerializeField] Image screenBlock;

    // The song to play
    [SerializeField] AudioSource song;

    // The box containing the names
    [SerializeField] GameObject nameBox;

    // The prompt in the lower right corner of the dialogue box indicating that a user can continue onto the next option
    [SerializeField] GameObject contPrompt;

    // The rate at which characters appear on the screen
    [SerializeField] int txtSpeed;

    [SerializeField] string nextScene;

    private Image spName;
    private Transform tfName;
    private RectTransform tfNameRect;
    private RectTransform tf;
    private AudioSource AuSo;
    private AudioSource textScrollBlip;
    private int size;
    private int at;
    private string[] script;
    private string toRead;
    private int reader;
    private bool sounding;
    private int waitTime;

    // Start is called before the first frame update
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;

        // Name Box Components
        spName = nameBox.GetComponent<Image>();
        tfName = nameBox.GetComponent<Transform>();
        tfNameRect = nameBox.GetComponent<RectTransform>();
        nameBox.SetActive(false);

        // Sound effects
        AuSo = Camera.main.GetComponent<AudioSource>();

        // Gets info on the dialogue box components
        textScrollBlip = GetComponent<AudioSource>();
        tf = GetComponent<RectTransform>();
        spName.color = new Color(1, 1, 1, 0);
        ReadNewScript(initialScriptName);
    }

    // Update is called once per frame
    void Update()
    {
        // Autoscroll Dialogue
        if (reader <= toRead.Length && waitTime == 0)
        {
            reader += txtSpeed;
            text.text = toRead.Substring(0, Mathf.Min(reader, toRead.Length));
            if (!textScrollBlip.isPlaying)
                textScrollBlip.Play();

            if (reader > toRead.Length && contPrompt.activeSelf == false)
                contPrompt.SetActive(true);
        }

        

        // Next Dialogue Line
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(0)) && reader >= toRead.Length && waitTime == 0)
        {
            // Disable the continue prompt
            contPrompt.SetActive(false);

            // Progress in the script
            at++;
            toRead = script[at];

            // Check for special Cases
            checkCase();

            // Reset the character reader
            reader = 0;
        }

        // Wait Time Check
        if (waitTime > 0)
        {
            waitTime--;
            if (waitTime == 0)
                checkCase();
        }
    }

    // External means to call a line in the script if we need
    public void ReadDialogue(int rowNum)
    {
        spName.color = new Color(1, 1, 1, 1);
        at = rowNum;
        toRead = script[at];
    }

    // Picks out a new script and begins reading it (if we need to do this)
    public void ReadNewScript(string scriptName)
    {
        script = Resources.Load<TextAsset>(scriptName).text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        toRead = script[0];
        reader = 0;
        at = 0;
    }

    // Case checker for special commands within the script
    void checkCase()
    {
        // Run this until we find ourselves at a command that does not match
        bool finished = false;
        while (toRead.Length >= 5 && !finished)
        {
            //Debug.Log(toRead);
            switch (toRead.Substring(0, 5))
            {
                case "ENDS:":   // Resets all values to reset and close the text box
                    text.text = "";
                    nametag.text = "";
                    spName.color = new Color(255, 255, 255, 0);
                    pic1.color = new Color(pic1.color.r, pic1.color.g, pic1.color.b, 0);
                    pic2.color = new Color(pic2.color.r, pic2.color.g, pic2.color.b, 0);
                    nameBox.SetActive(false);
                    pic1.sprite = null;
                    pic2.sprite = null;
                    tfName.position = new Vector3(tfName.parent.position.x - 9.5F, tfName.position.y, tfName.position.z);
                    finished = true;
                    LoadEntities.enemyStatFile = Resources.Load<TextAsset>("BattleSpawnFiles/" + nextScene);
                    SceneManager.LoadScene("BattleScene");
                    break;


                case "NAME:": // Sets the name of the speaking character to the proceeding name, swaps the side of the name box
                    if(toRead.Substring(5) != "")
                        nameBox.SetActive(true);
                    else
                        nameBox.SetActive(false);

                    // Reposition name box
                    if (nametag.text != "")
                    {
                        if (tfName.localPosition.x == -(tf.rect.width / 2))
                        {
                            tfNameRect.pivot = new Vector2(1, 0);
                            tfName.localPosition = new Vector3(tf.rect.width / 2, tfName.localPosition.y, tfName.localPosition.z);
                        }
                        else
                        {
                            tfNameRect.pivot = new Vector2(0, 0);
                            tfName.localPosition = new Vector3(-(tf.rect.width / 2), tfName.localPosition.y, tfName.localPosition.z);
                        }
                    }
                    else
                    {
                        spName.color = new Color(1, 1, 1, 1);
                    }


                    float col = 100F;
                    if (tfName.localPosition.x == -(tf.rect.width / 2))
                    {
                        if (pic1.color.a == 1)
                            pic1.color = new Color(255, 255, 255, 1);
                        if (pic2.color.a == 1)
                            pic2.color = new Color(col / 255F, col / 255F, col / 255F, 1);
                    }
                    else
                    {
                        if (pic1.color.a == 1)
                            pic1.color = new Color(col / 255F, col / 255F, col / 255F, 1);
                        if (pic2.color.a == 1)
                            pic2.color = new Color(255, 255, 255, 1);
                    }

                    nametag.text = toRead.Substring(5);

                    break;


                case "PIC1:": // Sets the sprite of the left character to the image named
                    pic1.sprite = Resources.Load<Sprite>("Sprites/" + toRead.Substring(5));
                    pic1.color = new Color(pic1.color.r, pic1.color.g, pic1.color.b, 1);
                    break;
                case "PIC2:": // Sets the sprite of the right character to the image named
                    pic2.sprite = Resources.Load<Sprite>("Sprites/" + toRead.Substring(5));
                    pic2.color = new Color(pic2.color.r, pic2.color.g, pic2.color.b, 1);
                    break;


                case "NOIS:": // Plays the noise specified by the proceeding text
                    AuSo.clip = Resources.Load<AudioClip>("Sounds/" + toRead.Substring(5));
                    AuSo.Play();
                    at++;
                    toRead = script[at];
                    break;


                case "FLTR:": // Applies a screen block color, will likely change this at some point to make it a material as well...
                    string[] colors = toRead.Substring(5).Split(',');
                    screenBlock.color = new Color(int.Parse(colors[0]) / 255F, int.Parse(colors[1]) / 255F, int.Parse(colors[2]) / 255F, float.Parse(colors[3]));
                    break;


                case "SONG:": // Starts to play the song specified
                    string[] command = toRead.Substring(5).Split(',');
                    song.clip = Resources.Load<AudioClip>("Sounds/" + command[1]);
                    if (command[0] == "Play")
                        song.Play();
                    else
                        song.Stop();
                    break;


                case "WAIT:": // Waits x amount of frames before proceeding to the next line
                    waitTime = (int) (float.Parse(toRead.Substring(5)) * 60);
                    at++;
                    toRead = script[at];
                    finished = true;
                    break;


                case "EMPT:": // Clears the text
                    text.text = "";
                    break;


                default: // If no cases matched, you are finished
                    finished = true;
                    break;
            }
            if (!finished)
            {
                at++;
                toRead = script[at];
            }
        }
    }
}
