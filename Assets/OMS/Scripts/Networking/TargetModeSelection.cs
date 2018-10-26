using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetModeSelection : MonoBehaviour 
{
    [SerializeField] GameObject[] vrObjects;
    [SerializeField] MonoBehaviour[] vrComponents;

    [SerializeField] GameObject[] standaloneObjects;
    [SerializeField] MonoBehaviour[] standaloneComponents;

    void Start()
    {
        SetMode(UnityEngine.XR.XRDevice.isPresent);
    }

    void SetMode(bool isVR)
    {
        foreach (GameObject go in vrObjects)
        {
            go.SetActive(isVR);
        }

        foreach (MonoBehaviour component in vrComponents)
        {
            component.enabled = isVR;
        }

        foreach (GameObject go in standaloneObjects)
        {
            go.SetActive(!isVR);
        }

        foreach (MonoBehaviour component in standaloneComponents)
        {
            component.enabled = !isVR;
        }
    }
}
