using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    [SerializeField] private TextMeshProUGUI fillText;

    public float Fill {set { fillImage.fillAmount = value; }  }

    // value 0 -> 1
    public void SetProgress(float value)
    {
        fillImage.fillAmount = Mathf.Clamp01(value);
    }

    public void SetText(string text)
    {
        fillText.text = text;
    }
}
