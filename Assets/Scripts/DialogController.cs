using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static DialogController instance;

    public Text dialogText;
    public float letterSpeed = 0.02f;

    private Coroutine currentDialogCoroutine;
    public bool isShowingDialog;

    private void Awake()
    {
        instance = this;
    }

    public void ShowDialog(string message)
    {   
        UIController.Instance.SetCrosshair(b: false);
        UIController.Instance.SetCrosshairFill(b: false);
        UIController.Instance.SetInteractableInfoTextActive(b: false);
        if (currentDialogCoroutine != null)
            StopCoroutine(currentDialogCoroutine);

        currentDialogCoroutine = StartCoroutine(ShowText(message));
    }

    private IEnumerator ShowText(string text)
    {
        isShowingDialog = true;
    
        dialogText.gameObject.SetActive(true);
        dialogText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            AudioController.Instance.SpawnDialogLetterTick();
            dialogText.text += letter;
            yield return new WaitForSeconds(letterSpeed);
        }

        yield return new WaitForSeconds(2f);

        HideDialog();
    }

    private void HideDialog()
    {
        if (currentDialogCoroutine != null)
            StopCoroutine(currentDialogCoroutine);

        dialogText.gameObject.SetActive(false);
        isShowingDialog = false;
        UIController.Instance.SetCrosshair(b: true);
    }
}
