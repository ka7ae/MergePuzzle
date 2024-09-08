using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    public float rotateSpeed, rotateAngle;

    void Update()
    {
        transform.Rotate(0, 0, rotateAngle * Time.deltaTime * rotateSpeed);
    }
}