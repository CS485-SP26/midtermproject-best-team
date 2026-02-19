using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    // value 0 -> 1
    public void SetProgress(float value)
    {
        fillImage.fillAmount = Mathf.Clamp01(value);
    }
}
