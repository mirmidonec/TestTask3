using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Text interactableInfoText;
    public GameObject crosshairFill;
    public GameObject crosshair;
    public GameObject buttonsHint;

    private void Awake()
    {
        Instance = this;
    }

    public void SetInteractableInfoText(string text)
    {
        interactableInfoText.text = text;
    }

    public void SetInteractableInfoTextActive(bool b)
    {
        if ((b || crosshairFill.activeInHierarchy) && (!b || !crosshairFill.activeInHierarchy))
        {
            interactableInfoText.gameObject.SetActive(b);
            if (!b)
            {
                SetInteractableInfoText("");
            }
            SetCrosshairFill(b);
        }
    }

    public void SetCrosshairFill(bool b)
    {
        crosshairFill.SetActive(b);
    }

    public void SetCrosshair(bool b)
    {
        crosshair.SetActive(b);
    }

    public void SetButtonsHint(bool b)
    {
        buttonsHint.SetActive(b);
    }
}
