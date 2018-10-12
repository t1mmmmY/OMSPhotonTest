using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenStats : MonoBehaviour 
{
    [SerializeField] TextMesh healthValue;

    void Start()
    {
        FakeOmsEngine.onChangeHealth += OnChangeHealth;
    }

    void OnDestroy()
    {
        FakeOmsEngine.onChangeHealth -= OnChangeHealth;
    }

    void OnChangeHealth(int newValue)
    {
        healthValue.text = newValue.ToString();
    }
}
