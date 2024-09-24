using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditButton : MonoBehaviour
{
    [SerializeField] Canvas explanationCanvas;
    [SerializeField] TextMeshProUGUI creditText;
    [SerializeField] Button creditButton;
    bool isClick = false;

    void Start()
    {
        explanationCanvas.enabled = false;
    }

    public void OnPressed()
    {
        if (isClick)
        {
            explanationCanvas.enabled = false;
            isClick = false;
            creditText.text = "遊び方＆クレジット";
        }

        else if (creditButton)
        {
            explanationCanvas.enabled = true;
            isClick = true;
            creditText.text = "閉じる";
        }
    }
}
