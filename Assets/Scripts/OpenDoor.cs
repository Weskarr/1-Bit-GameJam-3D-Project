using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class OpenDoor : MonoBehaviour, Interactable
{
    private Quaternion closedRotation;
    [SerializeField]
    private Vector3 openRotation;

    private Vector3 closedPosition;
    [SerializeField]
    private Vector3 openPosition;

    public bool rotate = true;
    public bool move = false;

    [SerializeField]
    private float moveSpeed = 2f; // How many seconds it takes to open/close

    bool close = true;
    bool moving = false;

    [Header("Wwise Events")]
    public AK.Wwise.Event openSound;

    [Header("Wwise Events")]
    public AK.Wwise.Event closeSound;

    float p;
    private void Start() // Assumes the object starts in the closed position
    {
        closedRotation = transform.rotation;
        closedPosition = transform.position;

        if (close)  p = 0;
        else        p = 1;
    }


    private void Update() {
        if (moving) {
            if (close)  p -= (1 / moveSpeed) * Time.deltaTime;
            else        p += (1 / moveSpeed) * Time.deltaTime;

            if (p > 1) {
                p = 1;
                moving = false;
            }
            if (p < 0) {
                p = 0;
                moving = false;
                closeSound.Post(gameObject);
            }
            if (rotate) transform.rotation = Quaternion.Lerp(closedRotation, Quaternion.Euler(openRotation), p);
            if (move)   transform.position = Vector3.Lerp(closedPosition, openPosition, p);
        }
    }


    public void Interact()
    {
        if (close && !moving) {
            openSound.Post(gameObject);
        }
        close = !close;
        moving = true;
    }

}
