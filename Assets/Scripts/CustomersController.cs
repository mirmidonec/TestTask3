using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomersController : MonoBehaviour
{
    public static CustomersController instance;

    public Transform startPos;
    public Transform kioskPos;
    public Transform endPos;

    public Customer[] customerPrefabs;

    private Customer currentCustomer;
    private int currentCustomerIndex = 0;


    [Header("Scary event")]
    public AudioSource rain;
    public AudioSource ambient;
    public AudioSource ScaryAmbient;
    public AudioSource Scream;
    public AudioSource Scream2;
    public AudioSource Scream3;
    public AudioSource Stab;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke("SpawnCustomer", 7f);
    }
    public void StartEvent()
    {
        StartCoroutine(ScaryEvent());
    }

    private IEnumerator ScaryEvent()
    {
        rain.Stop();
        ambient.Stop();
        yield return new WaitForSeconds(0.5f);
        ScaryAmbient.Play();
        yield return new WaitForSeconds(9f);
        PlayerController.Instance.isCameraShaking = true;
        Scream.Play();
    }
    public void EndGame()
    {
        StartCoroutine(EndGameRoutine());
    }
    private IEnumerator EndGameRoutine()
    {
        Scream.Stop();
        Scream2.Play();
        PlayerController.Instance.FocusViewOn(currentCustomer.headRotate);
        yield return new WaitForSeconds(0.1f);
        PlayerController.Instance.enabled = false;
        UIController.Instance.FadeIns();
        yield return new WaitForSeconds(3f);
        Stab.Play();
        yield return new WaitForSeconds(1f);
        Scream3.Play();
        yield return new WaitForSeconds(3f);
        DialogController.instance.ShowDialogOutro("Спасибо, что поиграли в мою реализацию вашего ТЗ, буду рад с вами поработать!");
        yield return new WaitForSeconds(8f);
        Application.Quit();
    }

    public void DestroyCurrentCustomer()
    {
        if (currentCustomer != null)
        {
            Destroy(currentCustomer.gameObject);
        }
    }

    public void SpawnCustomer()
    {
        DestroyCurrentCustomer();

        Customer customerToSpawn = customerPrefabs[currentCustomerIndex];
        currentCustomer = Instantiate(customerToSpawn, startPos.position, Quaternion.identity);
        currentCustomer.UpdateDestination(kioskPos);
    }

    public void NextCustomer()
    {
        currentCustomerIndex++;

        if (currentCustomerIndex < customerPrefabs.Length)
        {
            Invoke("SpawnCustomer", 2f);
        }
        else
        {
            Debug.Log("Все клиенты обслужены!");
        }
    }

    public Customer GetCurrentCustomer()
    {
        return currentCustomer;
    }
}
