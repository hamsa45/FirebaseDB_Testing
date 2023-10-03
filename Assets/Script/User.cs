using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    public string name;
    public float time;
    public int gold;

    public User(string _name, float _time, int _gold)
	{
        this.name = _name;
        this.time = _time;
        this.gold = _gold;
	}
}

public class User2
{
    public string ExpName;
    public string  totalTimeSpan;

    public User2(string _name, string timeSpan)
	{
        this.ExpName = _name;
        this.totalTimeSpan = timeSpan;
	}
}