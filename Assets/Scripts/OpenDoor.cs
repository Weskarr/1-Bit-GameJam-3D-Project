using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class OpenDoor : MonoBehaviour, Interactable
{
    private float startDoorRotation;
    [SerializeField]
    private float endDoorRotation;

    private float beginFromRotation;
    private float goToRotation;

    [SerializeField]
    private float timeInterval = 0.01f;
    [SerializeField]
    private float moveAmount = 5f;
    private float currentRotation;

    private bool endIsNegative = false;
    private bool isOpenOrClosing = false;
    private bool isOpen = false;
    private bool inMotion = false;

    [Header("Wwise Events")]
    public AK.Wwise.Event fuseOpen;

    private void Start()
    {
        startDoorRotation = transform.eulerAngles.y;

        if (endDoorRotation < 0)
            endIsNegative = true;
        else
            endIsNegative = false;
    }

    public void Interact()
    {

        if (!isOpenOrClosing && !isOpen) // Open
        {
            beginFromRotation = startDoorRotation;
            goToRotation = endDoorRotation;
            fuseOpen.Post(gameObject);
            isOpenOrClosing = true;
            inMotion = true;
        }
        else if (!isOpenOrClosing && isOpen) // Close
        {
            beginFromRotation = endDoorRotation;
            goToRotation = startDoorRotation;
            isOpenOrClosing = true;
            inMotion = true;
        }
        else if (isOpenOrClosing && !isOpen) // Interupt Opening
        {
            goToRotation = startDoorRotation;
            inMotion = true;
        }
        else if (isOpenOrClosing && isOpen) // Interupt Closing
        {
            goToRotation = endDoorRotation;
            inMotion = true;
        }

        StopAllCoroutines();
        StartCoroutine(RotateCoroutine());
    }

     IEnumerator RotateCoroutine()
    {
        float direction;
        float amount;

        if (goToRotation == 0)
        {
            if (endIsNegative == true)
            {
                direction = moveAmount;
                amount = 0;
            }
            else
            {
                direction = -moveAmount;
                amount = -0;
            }
        }
        else if (goToRotation > 0)
        {
            direction = moveAmount;
            amount = 90f;
        }
        else 
        {
            direction = -moveAmount;
            amount = -90f;
        }

        while (inMotion)
        {
            yield return new WaitForSeconds(timeInterval);
            currentRotation += direction;

            if (direction > 0 && currentRotation >= amount)
            {
                this.transform.rotation = Quaternion.Euler(0, goToRotation, 0);
                inMotion = false;

                if (goToRotation == endDoorRotation)
                    isOpen = true;
                else
                    isOpen = false;

                isOpenOrClosing = false;

            }
            else if (direction < 0 && currentRotation <= amount)
            {
                this.transform.rotation = Quaternion.Euler(0, goToRotation, 0);
                inMotion = false;

                if (goToRotation == endDoorRotation)
                    isOpen = true;
                else
                    isOpen = false;

                isOpenOrClosing = false;
            }

            this.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }

        Debug.Log("cycle");
    }
}
