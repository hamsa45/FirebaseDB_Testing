using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using TMPro;
using Firebase;
using System;
using Firebase.Extensions;

public class DB_manager : MonoBehaviour
{
    public TMP_InputField _nameInput;
    public TMP_InputField _count;
    public float _time = 2;
    
    private string headID = "SenseXR";
    private string Exp = "Experiences";
    private float expStartTime;

    private DatabaseReference dbRef;
    private FirebaseDatabase database;

    // Start is called before the first frame update
    void Start()
    {
        database = FirebaseDatabase.GetInstance("https://fir-databasetest2-e7e86-default-rtdb.firebaseio.com/");
        dbRef = database.RootReference;
    }
    
    public void updateUser()
	{
        ////User newUser = new User(_nameInput.text, _time, int.Parse(_count.text));
        //TimeSpan time2 = TimeSpan.FromSeconds(120f);
        //User2 newUser = new User2(_nameInput.text, time2.ToString());
        //string jsonString = JsonUtility.ToJson(newUser);

        ////dbRef.Child(headID).Child(Exp).SetRawJsonValueAsync(jsonString);
        //dbRef.Child(Exp).SetRawJsonValueAsync(jsonString);
        DatabaseReference expRef = dbRef.Child(Exp);
        getValue();
	}

    public void startExperience(string ExpName)
	{
        expStartTime = Time.time;
	}

    public void endExperience(string ExpName)
	{

	}

    private void getValue()
	{
        //database.GetReference(headID).Child(Exp).Child("ExpName").GetValueAsync().ContinueWithOnMainThread(task => 
        database.GetReference(Exp).GetValueAsync().ContinueWithOnMainThread(task => 
        {
           if (task.IsFaulted)
           {
                Debug.Log("error while getting reference of data");
           }
           else if (task.IsCompleted)
           {
               DataSnapshot snapshot = task.Result;
               _count.text = (string) snapshot.Value;
           }
        });
    }

    public void updateValue(string ExpName, string time)
	{

	}


}
