using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithValue : MonoBehaviour 
{
	[SerializeField] Text valueLabel;

	public void OnChangeValue(float value)
	{
		valueLabel.text = value.ToString();
	}
}
