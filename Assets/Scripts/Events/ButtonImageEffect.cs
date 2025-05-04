using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonImageEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image[] imagesToDarken; // Mảng chứa các Image
    private TMPro.TMP_Text[] textsToDarken; // Mảng chứa các TextMeshProUGUI
    private Color normalColor = Color.white;
    private Color pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    private void Awake()
    {
        // Tự động tìm tất cả Image trong Button và các con
        imagesToDarken = GetComponentsInChildren<Image>(true); // true để bao gồm cả các object không active

        textsToDarken = GetComponentsInChildren<TMPro.TMP_Text>(true); // true để bao gồm cả các object không active
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Image img in imagesToDarken)
        {
            img.color = pressedColor;
        }
        foreach (TMPro.TMP_Text text in textsToDarken)
        {
            text.color = pressedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (Image img in imagesToDarken)
        {
            img.color = normalColor;
        }
        foreach (TMPro.TMP_Text text in textsToDarken)
        {
            text.color = normalColor;
        }
    }
}