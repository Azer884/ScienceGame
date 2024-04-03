using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    [HideInInspector]public bool IsOpened = false;
    public GameObject Water;
    [SerializeField]private Vector3 OpennedPos;
    [SerializeField]private Vector3 ClosedPos;
    [SerializeField]private Quaternion OpennedRot;
    [SerializeField]private Quaternion ClosedRot;
    [SerializeField]private float Speed = 5f;
    
    void Start()
    {
        IsOpened = false;
    }
    void Update()
    {
        CheckStat();
    }
    void CheckStat()
    {
        if (!IsOpened)
        {
            transform.SetLocalPositionAndRotation(Vector3.Lerp(transform.localPosition, OpennedPos, Time.deltaTime * Speed), Quaternion.Slerp(transform.localRotation, OpennedRot, Time.deltaTime * Speed));
            if (Water != null)
            {
                Water.SetActive(false);
            }
        }
        else 
        {
            transform.SetLocalPositionAndRotation(Vector3.Lerp(transform.localPosition, ClosedPos, Time.deltaTime * Speed), Quaternion.Slerp(transform.localRotation, ClosedRot, Time.deltaTime * Speed));
            if (Water != null)
            {
                Water.SetActive(true);
            }
        }
    }

}
