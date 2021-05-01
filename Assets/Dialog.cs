using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public List<string> dialogText { set; get; }
    Button button;
    Text text;
    int index = 0;

    void Start()
    {
        if (dialogText == null || dialogText.Count == 0)
        {
            transform.gameObject.SetActive(false);
            return;
        }
        text = GetComponentInChildren<Text>();
        text.text = dialogText[index];
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Onclick);
    }

    void Onclick()
    {
        if (dialogText.Count > index + 1)
        {
            index++;
            text.text = dialogText[index];
            return;
        }
        transform.gameObject.SetActive(false);
    }
}
