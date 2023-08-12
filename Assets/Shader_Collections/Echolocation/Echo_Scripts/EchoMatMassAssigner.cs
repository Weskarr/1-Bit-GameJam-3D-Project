using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoMatMassAssigner : MonoBehaviour
{
    [SerializeField]
    private Material _echoMaterial;

    [SerializeField]
    private Material _defaultMaterial;

    [SerializeField]
    private List<Renderer> _rendererList = new List<Renderer>();

    public void AssignEchoMat()
    {
        foreach (var renderer in _rendererList)
        {
            renderer.material = _echoMaterial;
        }
    }

    public void UnassignEchoNat()
    {
        foreach (var renderer in _rendererList)
        {
            renderer.material = _defaultMaterial;
        }
    }
}
