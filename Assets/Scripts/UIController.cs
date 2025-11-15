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
    private float deltaTime = 0.0f;
    public Text fpsText;
    
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        
        if (fpsText != null)
        {
            fpsText.text = $"FPS: {Mathf.Round(fps)}";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

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
