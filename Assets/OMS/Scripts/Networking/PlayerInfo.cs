using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo 
{
	public int number;
	public string name;
    public bool isLocal;
    public bool isReady;

    public PlayerInfo(int number, string name, bool isLocal, bool isReady)
	{
		this.number = number;
		this.name = name;
        this.isLocal = isLocal;
        this.isReady = isReady;
	}
}
