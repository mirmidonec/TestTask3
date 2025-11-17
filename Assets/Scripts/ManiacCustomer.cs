using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManiacCustomer : Customer
{
    public string badCoffeeDialog;
    public float halfWayStopDistance = 4f;
    public float rushSpeed = 6f;

    private bool rushing;
    public GameObject Knife;

    public override void GoAway()
    {
        canInteract = false;
        followWithTheHead = false;

        UpdateDestination(CustomersController.instance.endPos);
        StartCoroutine(StopHalfWayAndInsult());
    }

    private IEnumerator StopHalfWayAndInsult()
    {
        while (Vector3.Distance(transform.position, CustomersController.instance.endPos.position) > halfWayStopDistance)
        {
            yield return null;
        }

        navMeshAgent.ResetPath();
        characterAnimator.SetBool("walk", false);

        yield return StartCoroutine(RotateToPlayer());
        CustomersController.instance.StartEvent();
        PlayerController.Instance.FocusViewOn(headRotate);
        DialogController.instance.ShowDialog(badCoffeeDialog);
        followWithTheHead = true;
        Knife.SetActive(true);

        yield return new WaitForSeconds(10f);

        StartRushToPlayer();
    }

    private IEnumerator RotateToPlayer()
    {
        float t = 0f;
        float duration = 0.6f;
        Vector3 startRot = transform.localEulerAngles;

        Vector3 targetDir = PlayerController.Instance.transform.position - transform.position;
        Vector3 endRot = new Vector3(0f, Quaternion.LookRotation(targetDir).eulerAngles.y, 0f);

        while (t < duration)
        {
            transform.localEulerAngles = Vector3.Lerp(startRot, endRot, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localEulerAngles = endRot;
    }

    private void StartRushToPlayer()
    {
        rushing = true;
        navMeshAgent.speed = rushSpeed;
        navMeshAgent.SetDestination(PlayerController.Instance.transform.position);
        characterAnimator.SetBool("walk", true);
    }

bool isEnd = false;
    private new void Update()
    {
        base.Update();

        if (rushing && navMeshAgent != null && !isEnd)
        {
            navMeshAgent.SetDestination(PlayerController.Instance.transform.position);
            if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= 1.5f)
            {
               navMeshAgent.SetDestination(transform.position);
                characterAnimator.SetBool("walk", false);
               CustomersController.instance.EndGame();
               isEnd = true;
            }
        }
    }

}
