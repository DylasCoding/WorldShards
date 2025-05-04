using System.Collections.Generic;
using UnityEngine;

public class SwitchImageGroupManager : MonoBehaviour
{
    public static SwitchImageGroupManager Instance;

    private List<SwitchImageButton> allButtons = new List<SwitchImageButton>();
    private SwitchImageButton currentActive = null;

    private void Awake()
    {
        Instance = this;
    }

    public void Register(SwitchImageButton button)
    {
        if (!allButtons.Contains(button))
            allButtons.Add(button);
    }

    public void SetActiveButton(SwitchImageButton selectedButton)
    {
        foreach (var btn in allButtons)
        {
            bool isThis = (btn == selectedButton);
            btn.SetState(isThis);

            if (isThis)
                currentActive = btn;
        }
    }
}
