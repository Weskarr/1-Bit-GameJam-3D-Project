using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private OpenDoor myDoor;
    [SerializeField]
    private OpenDoor mySecondDoor;
    [SerializeField]
    private bool isSpawning = false;
    [SerializeField]
    private float openCloseTimer = 0.5f;


    private float originalMoveSpeed;
    public void ParticlesOn()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        isSpawning = true;
        originalMoveSpeed = myDoor.moveSpeed;
        StartCoroutine(MessingWithMyDoor());
    }
    public void ParticlesOff()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
        isSpawning = false;
        myDoor.moveSpeed = originalMoveSpeed;
        if (mySecondDoor != null)
            mySecondDoor.moveSpeed = originalMoveSpeed;
        StopCoroutine(MessingWithMyDoor());
    }


    // Start is called before the first frame update
    void Start()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator MessingWithMyDoor()
    {
        while (isSpawning)
        {
            myDoor.moveSpeed = 0.5f;
            myDoor.Interact();
            if (mySecondDoor != null)
            {
                mySecondDoor.moveSpeed = 0.5f;
                mySecondDoor.Interact();
            }
            yield return new WaitForSeconds(Random.Range(0.5f, openCloseTimer));
        }
    }
}
