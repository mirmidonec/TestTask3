using System.Collections;
using System.Collections.Generic;
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



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnCustomer();
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
