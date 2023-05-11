using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureScript : MonoBehaviour
{
    private Transform tf;
    private SpriteRenderer sr;

    private void Start()
    {
        tf = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();

        sr.sortingOrder = 10 - (int)tf.position.y;
    }
}
