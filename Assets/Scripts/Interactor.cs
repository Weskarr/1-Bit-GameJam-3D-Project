using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface Interactable {
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float interactRange;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            bool hit = Physics.Raycast(r, out RaycastHit hitInfo, interactRange);
            if (hit) {
                if (hitInfo.collider.gameObject.TryGetComponent(out Interactable obj)) {
                    obj.Interact();
                }
            }
        }
    }
}
