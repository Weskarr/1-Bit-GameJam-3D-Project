using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour, Interactable
{
    [SerializeField]
    private LightSwitch2 mylightSwitch;

    private Quaternion closedRotation;
    [SerializeField]
    private Vector3 openRotation; // Relative

    private Vector3 closedPosition;
    [SerializeField]
    private Vector3 openPosition; // Relative

    public bool rotate = true; // For rotating based
    public bool move = false; // For sliding based
    public bool animate = false; // For animations based

    public bool playCloseInstantly = false;

    [SerializeField]
    private Animator animator; // Assign by hand!

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

        if (close) p = 0;
        else p = 1;

        if (animate)
        {
            //animator = this.GetComponent<Animator>();
            if (animator == null)
                Debug.LogError("No Animator to Animate with!");
        }
    }


    private void Update()
    {
        if (moving)
        {
            if (close) p -= (1 / moveSpeed) * Time.deltaTime;
            else p += (1 / moveSpeed) * Time.deltaTime;

            if (p > 1)
            {
                p = 1;
                moving = false;
                closePower();
            }
            if (p < 0)
            {
                p = 0;
                moving = false;
                openPower();
                if (!playCloseInstantly) closeSound.Post(gameObject);
            }
            if (rotate) transform.rotation = Quaternion.Lerp(closedRotation, closedRotation * Quaternion.Euler(openRotation), p); // * is + in quaternion rotations kinda
            if (move) transform.position = Vector3.Lerp(closedPosition, closedPosition + openPosition, p);
            if (animate) animator.SetFloat("blend", p); // Animator uses a float value in a blend tree = (0 == closed) && (1 == open)
        }
    }

    public void BreakerIsOut()
    {
        if (close && !moving)
        {
            openSound.Post(gameObject);
        }
        close = !close;
        moving = true;
    }

    public void Interact()
    {
        if (close && !moving)
        {
            openSound.Post(gameObject);
        }
        if (!close && !moving && playCloseInstantly)
        {
            closeSound.Post(gameObject);
        }
        close = !close;
        moving = true;
    }

    public void closePower()
    {
        mylightSwitch.powerIsGone();
    }

    public void openPower()
    {
        mylightSwitch.powerIsBack();
    }
}
