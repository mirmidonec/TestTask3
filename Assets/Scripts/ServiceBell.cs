using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceBell : Interactable
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public override void PressVirtual()
	{
		base.PressVirtual();
		BellRing();
	}

	private void OnCollisionEnter(Collision other)
	{
		BellRing();
	}

	public void BellRing()
	{
		AudioController.Instance.SpawnRingAtPos(this.transform.position);
        anim.Play("BellRingAnim");

	}
}
