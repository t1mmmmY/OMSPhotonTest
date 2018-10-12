using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveObjectHint : MonoBehaviour 
{
    [SerializeField] GameObject hint;
    [SerializeField] Text hintLabel;

    public void ShowHint(ActiveObject activeObject)
    {
        hint.SetActive(true);
        hintLabel.text = activeObject.actionTextKey;
    }

    public void HideHint()
    {
        hint.SetActive(false);
    }
}