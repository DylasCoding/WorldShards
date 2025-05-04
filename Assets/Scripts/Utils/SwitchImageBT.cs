using UnityEngine;
using UnityEngine.UI;

public class SwitchImageButton : MonoBehaviour
{
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    public bool isSwitched = false;

    public void SwitchImage()
    {
        // Lấy component Button từ chính GameObject này
        Button button = GetComponent<Button>();

        Debug.Log("Clicked" + button);

        if (button != null && button.image != null)
        {
            button.image.sprite = isSwitched ? sprite1 : sprite2;
            isSwitched = !isSwitched;
        }
        else
        {
            Debug.LogWarning("Can't find image or button!");
        }
    }
}