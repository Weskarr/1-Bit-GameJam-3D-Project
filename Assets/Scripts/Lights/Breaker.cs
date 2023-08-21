using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour, Interactable
{
    public FuseBox fuseBox;

    [SerializeField]
    private LightSwitch2 mylightSwitch;

    public bool powered = true;

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

    AudioSource audioSource;
    float p;
    private void Start() // Assumes the object starts in the closed position
    {
        audioSource = GetComponent<AudioSource>();

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
                if (!playCloseInstantly) audioSource.Play();
            }
            if (rotate) transform.rotation = Quaternion.Lerp(closedRotation, closedRotation * Quaternion.Euler(openRotation), p); // * is + in quaternion rotations kinda
            if (move) transform.position = Vector3.Lerp(closedPosition, closedPosition + openPosition, p);
            if (animate) animator.SetFloat("blend", p); // Animator uses a float value in a blend tree = (0 == closed) && (1 == open)
        }
    }

    public void BreakerIsOut()
    {
        p = 0;
        close = false;
        moving = true;
    }

    public void Interact()
    {
        if (close && !moving)
        {
            audioSource.Play();
        }
        if (!close && !moving && playCloseInstantly)
        {
            audioSource.Play();
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
        fuseBox.powered = true;
        powered = true;
        mylightSwitch.powerIsBack();
    }
}
