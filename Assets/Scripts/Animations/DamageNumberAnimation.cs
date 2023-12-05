using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumberAnimation : MonoBehaviour
{
    public bool critical;
    public int value;
    private Color flashColor = new Color(1, 0, 0, 1);
    private Color white = new Color(1, 1, 1, 1);
    private float timer;
    private int cycles;
    private float cycleTime = 0.15f;
    private bool flashing;
    [SerializeField] int totalCycles;
    private TMP_Text text;
    private Vector3 direction;
    private Vector3 basePosition;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        if(critical)
        {
            flashColor = new Color(1, 1, 0, 1);
            text.color = flashColor;
            flashing = true;
        }
        float randomDirection = Random.Range(60, 120) * Mathf.PI / 180f;
        direction = new Vector3(Mathf.Cos(randomDirection), Mathf.Sin(randomDirection), 0);
        string damageText = "";
        int counter = 0;
        if (value == 0)
        {
            text.text = "NULL";
            text.color = new Color(0, 0, 1, 1);
            direction = new Vector3(0, 1, 0);
        }
        while(value > 0)
        {
            if(counter == 3)
            {
                damageText = "," + damageText;
                counter = 0;
            }
            damageText = (value % 10) + damageText;
            counter++;
            value /= 10;
        }
        if(text.text != "NULL")
            text.text = damageText;
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > cycleTime)
        {
            if(cycles == totalCycles)
            {
                Destroy(this.gameObject);
                return;
            }
            timer = 0;
            if (text.text != "NULL")
            {
                if (flashing)
                {
                    text.color = white;
                }
                else
                {
                    text.color = flashColor;
                }
                flashing = !flashing;
            }
            cycles++;

        }
        float progress = (cycles * cycleTime + timer) / (totalCycles * cycleTime);
        transform.position = Vector3.Lerp(basePosition, basePosition + direction, progress);
    }
}
