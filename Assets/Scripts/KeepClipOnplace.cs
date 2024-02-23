using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepClipOnplace : MonoBehaviour
{
    public GameObject Hand;

    public bool IsThere = false;
    void Update()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).transform.SetLocalPositionAndRotation(Vector3.zero , Quaternion.Euler(0f, 0f, 0f));
            IsThere = true;
        }
    }
}
