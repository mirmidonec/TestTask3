using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
public string interactableName;

	public bool canInteract = true;

	public bool noOutline;

	public Renderer[] allObjectMeshes;

	private void Start()
	{
		gameObject.layer = 6;
		SecondStart();
	}

	public virtual void SecondStart()
	{
	}

	public void GetChildMeshes()
	{
		allObjectMeshes = GetComponentsInChildren<Renderer>(includeInactive: true);
	}

	public async void SetMeshesLayer(int layerID)
	{
		if (allObjectMeshes != null && allObjectMeshes.Length != 0)
		{
			for (int i = 0; i < allObjectMeshes.Length; i++)
			{
				if (allObjectMeshes[i] != null && (allObjectMeshes[i].GetComponent<Interactable>() == null || allObjectMeshes[i].GetComponent<GrabObject>() != null))
				{
					allObjectMeshes[i].gameObject.layer = layerID;
				}
			}
		}
		if (!noOutline)
		{
			gameObject.layer = layerID;
		}
	}

	public void SetInteractableName(string newName)
	{
		interactableName = newName;
	}

	public void Press()
	{
		if (canInteract)
		{
			PressVirtual();
		}
	}

	public void Release()
	{
		if (canInteract)
		{
			ReleaseVirtual();
		}
	}

	public void Drag()
	{
		if (canInteract)
		{
			DragVirtual();
		}
	}

	public void StartUse()
	{
		if (canInteract)
		{
			StartUseVirtual();
		}
	}

	public void Use()
	{
		if (canInteract)
		{
			UseVirtual();
		}
	}

	public void StopUse()
	{
		if (canInteract)
		{
			StopUseVirtual();
		}
	}

	public void SetCanInteract(bool b)
	{
		canInteract = b;
	}

	public bool isInteractable()
	{
		return canInteract;
	}

	public virtual void PressVirtual()
	{
	}

	public virtual void ReleaseVirtual()
	{
	}

	public virtual void DragVirtual()
	{
	}

	public virtual void UseVirtual()
	{
	}

	public virtual void StopUseVirtual()
	{
	}

	public virtual void StartUseVirtual()
	{
	}
}
