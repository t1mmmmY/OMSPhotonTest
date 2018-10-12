using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : ActiveObject 
{
    [SerializeField] string actionKey;
    Animator buttonAnimator;

    void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    public override void DoAction()
    {
        FakeOmsEngine.Instance.DoAction(actionKey, this);
    }

    public override void PerformAction()
    {
        buttonAnimator.SetTrigger("Click");
    }
}
