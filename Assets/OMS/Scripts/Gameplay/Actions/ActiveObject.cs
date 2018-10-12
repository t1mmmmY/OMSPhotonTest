using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveObject : MonoBehaviour 
{
    public int actionNumber = 0;

    [SerializeField] string _actionTextKey;
    public string actionTextKey { get { return _actionTextKey; }}

    public abstract void DoAction();
    public abstract void PerformAction();
}