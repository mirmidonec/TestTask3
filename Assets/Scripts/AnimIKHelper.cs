using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimIKHelper : MonoBehaviour
{
	public Customer customerObj;

	private void OnAnimatorIK(int layerIndex)
	{
		customerObj.OnAnimatorIK(layerIndex);
	}
}
