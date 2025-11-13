using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : GrabObject
{
    public Transform lidPosition;

    public SkinnedMeshRenderer liquidMesh;

    public ParticleSystem fakeCoffeeParticle;

    public void SetBreakParticle()
    {
        breakable = true;
        breakParticle = fakeCoffeeParticle;
    }

    public void PourCoffee()
    {
        liquidMesh.SetBlendShapeWeight(0, 100f);
    }

    public void SetCoffeeValue(float value)
    {
        liquidMesh.SetBlendShapeWeight(0, value);
    }

    public void CoffeeCompleted()
    {
        interactableName = "Coffee";
        SetBreakParticle();
    }
}
