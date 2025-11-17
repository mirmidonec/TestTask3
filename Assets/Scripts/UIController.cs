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
    public Image fadeImage;
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
    private void Start()
    {
        StartCoroutine(FadeOut());
    }


    public void FadeIns()
    {
        StartCoroutine(FadeIn());
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

    public IEnumerator FadeIn(float duration = 1f)
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        fadeImage.gameObject.SetActive(true);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            fadeImage.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }

        fadeImage.color = targetColor;
    }
    public IEnumerator FadeOut(float duration = 1f)
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            fadeImage.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }

        fadeImage.color = targetColor;
        fadeImage.gameObject.SetActive(false);
    }
}
