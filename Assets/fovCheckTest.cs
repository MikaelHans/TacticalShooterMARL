using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fovCheckTest : MonoBehaviour
{
    public Character c;

    void Update()
    {
        c.fovCheck(gameObject);
    }
}
