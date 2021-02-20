using System;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : Singleton<ThemeManager>
{
    public enum Theme
    {
        Light = 0,
        Dark = 1
    }

    public Image Background;

    public Color MainBackground { get; private set; }
    public Color MainText { get; private set; }
    public Color SubText { get; private set; }
    public Color ButtonBackground { get; private set; }
    public Color ButtonText { get; private set; }
    public Color NavBackground { get; private set; }
    public Color NavMainText { get; private set; }
    public Color NavSubText { get; private set; }
    public Color NavButtonBackground { get; private set; }
    public Color NavButtonText { get; private set; }
    public Color NavButtonDisable { get; private set; }

    public void Init(Theme theme)
    {
        switch (theme)
        {
            case Theme.Light:
                MainBackground = new Color32(255, 255, 255, 255);
                MainText = new Color32(50, 50, 50, 255);
                SubText = new Color32(50, 50, 50, 128);
                ButtonBackground = new Color32(20, 155, 207, 255);
                ButtonText = new Color32(255, 255, 255, 255);
                NavBackground = new Color32(240, 240, 240, 255);
                NavMainText = new Color32(50, 50, 50, 255);
                NavSubText = new Color32(50, 50, 50, 128);
                NavButtonBackground = new Color32(20, 155, 207, 255);
                NavButtonText = new Color32(20, 155, 207, 255);
                NavButtonDisable = new Color32(50, 50, 50, 128);
                break;
            case Theme.Dark:
                MainBackground = new Color32(24, 25, 29, 255);
                MainText = new Color32(255, 255, 255, 255);
                SubText = new Color32(108, 108, 108, 255);
                ButtonBackground = new Color32(0, 155, 207, 255);
                ButtonText = new Color32(255, 255, 255, 255);
                NavBackground = new Color32(58, 64, 71, 255);
                NavMainText = new Color32(255, 255, 255, 255);
                NavSubText = new Color32(108, 108, 108, 255);
                NavButtonBackground = new Color32(0, 155, 207, 255);
                NavButtonText = new Color32(255, 255, 255, 255);
                NavButtonDisable = new Color32(108, 108, 108, 255);
                break;
        }

        Background.color = MainBackground;
    }
}