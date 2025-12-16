using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenMessage : MonoBehaviour
{
    public GameObject Panel;
    public TMP_Text MessageText;
    public GameObject OKButton;

    public void Show(string message, bool showButton)
    {
        MessageText.text = message;
        if (OKButton != null && !showButton)
        {
            OKButton.SetActive(false);
        }
        Panel.SetActive(true);
    }

    public void Hide()
    {
        Panel.SetActive(false);
    }
}