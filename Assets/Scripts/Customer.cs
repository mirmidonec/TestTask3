using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : TriggerObject
{
    public int customerID;
    public Animator characterAnimator;
    public Transform headRotate;

    private Transform destination;
    private NavMeshAgent navMeshAgent;
    private Coroutine tempCor;
    private bool ordered;
    private bool followWithTheHead;
    private float lookAtWeight = 0.5f;

    public int desiredItemID;
    public string approachDialog;
    public string successDialog;

    private GameObject serviceBell;
    private ServiceBell bell;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        serviceBell = GameObject.FindGameObjectWithTag("service");
        bell = serviceBell.GetComponent<ServiceBell>();
        canInteract = false;
    }

    public void OnAnimatorIK(int layerIndex)
    {
        if (followWithTheHead)
        {
            characterAnimator.SetLookAtPosition(PlayerController.Instance.MainCamera.transform.position + new Vector3(0f, -0.2f, 0f));
            characterAnimator.SetLookAtWeight(lookAtWeight);
        }
        else
        {
            characterAnimator.SetLookAtWeight(0f);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(destination.position, transform.position) < 0.2f && characterAnimator.GetBool("walk"))
        {
            characterAnimator.SetBool("walk", false);
            if (tempCor != null)
            {
                StopCoroutine(tempCor);
            }
            tempCor = StartCoroutine(RotateToTarget(90f));
        }
    }

    private IEnumerator RotateToTarget(float newAngle)
    {
        float lerpValue = 0f;
        float time = 0.6f;
        Vector3 startRot = transform.localEulerAngles;
        Vector3 endRot = new Vector3(0f, newAngle, 0f);

        while (lerpValue <= time)
        {
            transform.localEulerAngles = AngleLerp(startRot, endRot, lerpValue / time);
            lerpValue += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = endRot;
        tempCor = null;
        followWithTheHead = true;

        gameObject.layer = 6;
        canInteract = true;
        bell.BellRing();

        lerpValue = 0f;
        lookAtWeight = 0f;
        while (lerpValue <= time)
        {
            lookAtWeight = Mathf.Lerp(0f, 1f, lerpValue / time);
            lerpValue += Time.deltaTime;
            yield return null;
        }
        lookAtWeight = 1f;
    }

    private Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        float x = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float y = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        float z = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        return new Vector3(x, y, z);
    }

    public override void OnTriggerOrCollision(GrabObject tempGrabObj)
    {
        base.OnTriggerOrCollision(tempGrabObj);

        if (tempGrabObj != null && ordered && tempGrabObj.grabObjectID == desiredItemID)
        {
            // Заказ выполнен!
            Destroy(tempGrabObj.gameObject);
            CompleteOrder();
        }
    }

    private void CompleteOrder()
    {
        canInteract = false;
        PlayerController.Instance.FocusViewOn(headRotate);
        DialogController.instance.ShowDialog(successDialog);

        characterAnimator.CrossFade("Give", 0.2f);

        Invoke("GoAway", 2f);
    }

    public void GoAway()
    {
        canInteract = false;
        followWithTheHead = false;
        UpdateDestination(CustomersController.instance.endPos);

        StartCoroutine(WaitUntilGone());
    }

    private IEnumerator WaitUntilGone()
    {
        while (Vector3.Distance(transform.position, CustomersController.instance.endPos.position) > 2f)
        {
            yield return null;
        }
        Destroy(gameObject);
        CustomersController.instance.NextCustomer();
    }


    public void UpdateDestination(Transform newDestination)
    {
        if (navMeshAgent != null)
        {
            destination = newDestination;
            characterAnimator.SetBool("walk", true);
            navMeshAgent.SetDestination(destination.position);
        }
    }

    public override void PressVirtual()
    {
        base.PressVirtual();
        if (!ordered)
        {
            DialogController.instance.ShowDialog(approachDialog);
            PlayerController.Instance.FocusViewOn(headRotate);
            ordered = true;
        }
    }

    public bool DidOrder()
    {
        return ordered;
    }
}
