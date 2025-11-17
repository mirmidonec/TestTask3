using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance;
    public GrabObject currentGrabObj;
    private Interactable lastInteractable;
    private Interactable hoverInteractable;
    [Header("Interactable Values")]
    public LayerMask interactableMask;

    public LayerMask pointMask;

    public float interactableRange = 5f;
    public Transform grabHolder;
    public UnityEvent<GrabObject> onGrabObjEquip;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (DialogController.instance.isShowingDialog)
        {
            UIController.Instance.SetInteractableInfoTextActive(b: false);
            return;
        }
        if (Input.GetMouseButtonDown(0) && hasGrabObj())
        {
            currentGrabObj.Throw();
            SetGrabObj(null);
        }
        else if (Input.GetMouseButtonDown(1) && hasGrabObj())
        {
            currentGrabObj.Throw(addForce: false);
            SetGrabObj(null);
        }
        if (hasGrabObj())
        {
            return;
        }
        GameObject gameObject = ShootRay();
        if (gameObject != null && gameObject.GetComponent<Interactable>() != null)
        {
            if (hoverInteractable != null && hoverInteractable.gameObject != gameObject && hoverInteractable.isInteractable() && hoverInteractable.enabled && hoverInteractable.gameObject.layer == 8)
            {
                hoverInteractable.SetMeshesLayer(6);
                UIController.Instance.SetInteractableInfoTextActive(b: false);
                hoverInteractable = null;
            }
            if (getActiveInteractableObject(gameObject) != null && getActiveInteractableObject(gameObject).isInteractable())
            {
                hoverInteractable = getActiveInteractableObject(gameObject);
            }
        }
        else
        {
            UIController.Instance.SetInteractableInfoTextActive(b: false);
        }
        if (lastInteractable != null && lastInteractable.enabled)
        {
            if (Input.GetMouseButtonUp(0))
            {
                lastInteractable.Release();
                lastInteractable = null;
            }
            if (Input.GetMouseButton(0) && lastInteractable != null)
            {
                lastInteractable.Drag();
            }
            if (!Input.GetMouseButton(0) && lastInteractable != null)
            {
                lastInteractable = null;
            }
        }
        else if (gameObject != null && gameObject.GetComponent<Interactable>() != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hoverInteractable != null && hoverInteractable.isInteractable() && hoverInteractable.enabled)
                {
                    hoverInteractable.SetMeshesLayer(6);
                    UIController.Instance.SetInteractableInfoTextActive(b: false);
                    hoverInteractable = null;
                }
                if (getActiveInteractableObject(gameObject) != null)
                {
                    lastInteractable = getActiveInteractableObject(gameObject);
                    if (lastInteractable.enabled)
                    {
                        lastInteractable.Press();
                    }
                }
            }
            if (hoverInteractable != null && hoverInteractable.isInteractable() && hoverInteractable.enabled)
            {
                if (hoverInteractable.gameObject.layer == 6)
                {
                    hoverInteractable.SetMeshesLayer(8);
                    UIController.Instance.SetInteractableInfoText(hoverInteractable.interactableName);
                    UIController.Instance.SetInteractableInfoTextActive(hoverInteractable.interactableName != "");
                }
                else if (hoverInteractable.interactableName == "")
                {
                    UIController.Instance.SetInteractableInfoTextActive(b: false);
                }
            }
        }
        else if (hoverInteractable != null && hoverInteractable.isInteractable() && hoverInteractable.enabled)
        {
            if (hoverInteractable.gameObject.layer == 8)
            {
                hoverInteractable.SetMeshesLayer(6);
                UIController.Instance.SetInteractableInfoTextActive(b: false);
                hoverInteractable = null;
            }
        }
        else if (hoverInteractable != null)
        {
            UIController.Instance.SetInteractableInfoTextActive(b: false);
            hoverInteractable = null;
        }
        else
        {
            UIController.Instance.SetInteractableInfoTextActive(b: false);
        }
    }
    private Interactable getActiveInteractableObject(GameObject tempObj)
    {
        Interactable[] components = tempObj.GetComponents<Interactable>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].enabled)
            {
                return components[i];
            }
        }
        return null;
    }
    public bool hasGrabObj()
    {
        return currentGrabObj != null;
    }

    public void SetGrabObj(GrabObject tempGrabObj)
    {
        currentGrabObj = tempGrabObj;
        UIController.Instance.SetButtonsHint(tempGrabObj != null);
    }
    public bool isCurrentGrabObject(GrabObject thisGrabObj)
    {
        return currentGrabObj == thisGrabObj;
    }
    public void EquipGrabObj(GrabObject tempGrabObj)
    {
        if (!hasGrabObj())
        {
            SetGrabObj(tempGrabObj);
            tempGrabObj.Equip(grabHolder);
            onGrabObjEquip.Invoke(tempGrabObj);
        }
    }
    public GameObject ShootRay()
    {
        if (Physics.Raycast(PlayerController.Instance.MainCamera.transform.position, PlayerController.Instance.MainCamera.transform.TransformDirection(Vector3.forward), out var hitInfo, 2.4f, interactableMask, QueryTriggerInteraction.Ignore))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
}
