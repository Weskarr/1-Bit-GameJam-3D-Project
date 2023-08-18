using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class EchoManager : MonoBehaviour
{

    private static EchoManager echo;

    public static EchoManager instance {
        get {
            return echo;
        }
    }

    void OnEnable() { echo = this; }

    void OnDisable() { echo = null; }

    public GameObject player;

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
    public bool lightsOff = false;
    [SerializeField]
    private Transform _objects;
    [SerializeField]
    private Material _lightsOnMaterial;
    [SerializeField]
    private Material _lightsOffMaterial;

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

    public bool LightsOff()
    {
        player.GetComponent<Player>().DisableHands();

        // Return whether this actually changed something
        bool ret = !lightsOff;
        lightsOff = true;
        RecTrav(_objects, _lightsOffMaterial);
        ResetEcho();
        return ret;
    }

    public bool LightsOn()
    {
        player.GetComponent<Player>().EnableHands();

        // Return whether this actually changed something
        bool ret = lightsOff;
        lightsOff = false;
        RecTrav(_objects, _lightsOnMaterial);
        ResetEcho();
        return ret;
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E = to turn the lights off
        {
            if (lightsOff == true)
            {
                LightsOn();
            }
            else
            {
                LightsOff();
            }
        }

        if (lightsOff == true && Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Q)) // Spacebar or Q = to echo
        {
            
            if (!_isEchoing){
                StartEcho();
            }
               
        }
    }

    public void StartEcho() {
        ResetEcho();
        StartCoroutine(EchoExpansion());
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

    void RecTrav(Transform parent, Material material_to_apply) {
        foreach (Transform child in parent) {
            if (child.TryGetComponent<Renderer>(out Renderer renderer)) {
                Material[] mats = renderer.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                    mats[i] = material_to_apply;
                renderer.sharedMaterials = mats;
            }
            RecTrav(child, material_to_apply);
        }
    }
}
