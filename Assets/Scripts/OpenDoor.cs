using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class OpenDoor : MonoBehaviour, Interactable
{
    private Quaternion closedRotation;
    [SerializeField]
    private Vector3 openRotation; // Relative

    private Vector3 closedPosition;
    [SerializeField]
    private Vector3 openPosition; // Relative

    public bool rotate = true;
    public bool move = false;

    public bool playCloseInstantly = false;

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
                if (!playCloseInstantly) closeSound.Post(gameObject);
            }
            if (rotate) transform.rotation = Quaternion.Lerp(closedRotation, closedRotation * Quaternion.Euler(openRotation), p); // * is + in quaternion rotations kinda
            if (move)   transform.position = Vector3.Lerp(closedPosition, closedPosition + openPosition, p);
        }
    }


    public void Interact()
    {
        if (close && !moving) {
            openSound.Post(gameObject);
        }
        if (!close && !moving && playCloseInstantly) {
            closeSound.Post(gameObject);
        }
        close = !close;
        moving = true;
    }

}
