using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class EchoManager : MonoBehaviour
{


    // For echo expansion
    [SerializeField]
    private float _echoExpansionSpeed = 0.01f;
    [SerializeField]
    private float _echoExpansionAmount = 0.025f;

    // for echo radius
    [SerializeField]
    private float _echoCurRadius = 0f;
    [SerializeField]
    private float _echoMaxRadius = 50;

    // for echo fade
    [SerializeField]
    private float _echoColorPercentage;

    // for location
    [SerializeField]
    private GameObject _location;

    // Lights are on or off?
    [SerializeField]
    private bool _lightsOff = false;
    [SerializeField]
    private EchoMatMassAssigner _echoMatMassAssigner;

    // Extra
    [SerializeField]
    private bool _isEchoing = false;
    [SerializeField]
    private Material _echoMat;

    private void Start()
    {
        if (_location == null) // Must be assigned by hand!
            Debug.LogError("You need to assign: EchoManager/_location");

        if (_echoMat == null) // Must be assigned by hand!
            Debug.LogError("You need to assign: EchoManager/_echoMat");
    }

    private void OnApplicationQuit()
    {
        _echoMat.SetFloat("_Radius", 0f); // Make sure you set to 0, otherwise changes stick after game is ended.
    }

    private void LightsOff()
    {
        _lightsOff = true;
        _echoMatMassAssigner.AssignEchoMat();
        ResetEcho();
    }

    private void LightsOn()
    {
        _lightsOff = false;
        _echoMatMassAssigner.UnassignEchoNat();
        ResetEcho();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E = to turn the lights off
        {
            if (_lightsOff == true)
            {
                LightsOn();
            }
            else
            {
                LightsOff();
            }
        }

        if (_lightsOff == true && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Q)) // Spacebar or Q = to echo
        {
            ResetEcho();
            if (_isEchoing == false)
                StartCoroutine(EchoExpansion());
        }
    }

    private void ResetEcho()
    {
        StopAllCoroutines();
        _echoCurRadius = 0f;
        _echoMat.SetFloat("_Radius", _echoCurRadius);
        _isEchoing = false;
    }

    IEnumerator EchoExpansion()
    {
        // In case you want to check if coroutine is happening...
        _isEchoing = true;

        // for location
        _echoMat.SetVector("_Center", _location.transform.position);

        while (_isEchoing)
        {
            yield return new WaitForSeconds(_echoExpansionSpeed);
            
            // for radius expansion
            _echoCurRadius += _echoExpansionAmount;
            _echoMat.SetFloat("_Radius", _echoCurRadius);

            // For color fade
            _echoColorPercentage = 100 / _echoMaxRadius * _echoCurRadius / 100;
            _echoMat.color = Color.Lerp(Color.white, Color.black, _echoColorPercentage);

            // for max radius
            if (_echoCurRadius > _echoMaxRadius)
                ResetEcho();
        }

        _isEchoing = false;
    }
}