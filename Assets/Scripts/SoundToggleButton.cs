using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public enum ButtonType
    {
        BackgroundMusic,
        SoundFX
    };

    [SerializeField] private ButtonType buttonType;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private GameObject button;
    [SerializeField] private Vector3 offButtonPosition;

    private Vector3 onButtonPosition;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = onSprite;
        onButtonPosition = button.GetComponent<RectTransform>().anchoredPosition;

        ToggleButton();
    }

    public void ToggleButton()
    {
        var muted = false;

        if (buttonType == ButtonType.BackgroundMusic)
            muted = AudioManager.Instance.IsBackgroundMusicMuted();
        else
        {
            muted = AudioManager.Instance.IsSoundFxMuted();
        }

        if (muted)
        {
            image.sprite = offSprite;
            button.GetComponent<RectTransform>().anchoredPosition = offButtonPosition;
        } else
        {
            image.sprite = onSprite;
            button.GetComponent<RectTransform>().anchoredPosition = onButtonPosition;
        }
    }
}
