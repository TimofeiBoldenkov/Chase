using TMPro;
using UnityEngine;

public class VictoryMessage : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text messageText;

    public void Show(string message)
    {
        messageText.text = message;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
