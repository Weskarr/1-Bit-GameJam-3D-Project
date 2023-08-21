using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unstuck : MonoBehaviour
{
    public float stuckSeconds;
    public float jiggle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckStuck());
    }
    Vector3 lastPos = Vector3.zero;
    IEnumerator CheckStuck() {
        yield return new WaitForSeconds(stuckSeconds);
        int sign = RandomSign();
        if (transform.position == lastPos) {
            transform.position += new Vector3(sign * jiggle, 0, sign * jiggle);
        }
        lastPos = transform.position;
        StartCoroutine(CheckStuck());
    }

    int RandomSign() {
        return Random.value < .5 ? 1 : -1;
    }
}

