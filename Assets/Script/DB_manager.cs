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
    FirebaseApp app;
    private FirebaseDatabase database;

    // Start is called before the first frame update
    void Start()
	{
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
        //{
        //    if (task.IsFaulted && task.Exception != null)
		//	{
        //        Debug.Log(" encountered fault ");
		//	}
        //    else if(task.IsCompleted)
		//	{
        //        app = FirebaseApp.DefaultInstance;
        //        app.Options.DatabaseUrl = new Uri("https://fir-databasetest2-e7e86-default-rtdb.firebaseio.com/");
        //        //FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri("https://fir-databasetest2-e7e86-default-rtdb.firebaseio.com/");
        //    }
        //});
        StartCoroutine(databaseInstaceInit());   
    }

	IEnumerator databaseInstaceInit()
	{
        yield return new WaitForSeconds(1f);
        Debug.Log(" database instance getting ++++");
        //database = FirebaseDatabase.DefaultInstance;
        database = FirebaseDatabase.GetInstance("https://fir-databasetest2-e7e86-default-rtdb.firebaseio.com/");
        dbRef = database.RootReference;
        startExperience();
    }

	public void updateUser()
	{
        ////User newUser = new User(_nameInput.text, _time, int.Parse(_count.text));
        //TimeSpan time2 = TimeSpan.FromSeconds(120f);
        //User2 newUser = new User2(_nameInput.text, time2.ToString());
        //string jsonString = JsonUtility.ToJson(newUser);

        ////dbRef.Child(headID).Child(Exp).SetRawJsonValueAsync(jsonString);
        ////dbRef.Child(Exp).SetRawJsonValueAsync(jsonString);
        //DatabaseReference expRef = dbRef.Child(Exp);
        
        //getValue();
	}

    private void startExperience()
	{
        expStartTime = Time.time;
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

    public void checkAndSetExperienceUseTime(String ExpName)
    {
        //database.GetReference(headID).Child(Exp).Child("ExpName").GetValueAsync().ContinueWithOnMainThread(task => 
        database.GetReference(Exp).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log($" {ExpName} is the name provided and task completed");
            if (ExpName.Length == 0) ExpName = _nameInput.text;
            //if (ExpName == null)
            //{
            //    Debug.Log("experince name field is Empty");
            //    return;
            //}

            ExpName = ExpName.ToLower();

            float expUseTime = Time.time - expStartTime;
            TimeSpan totalTimeSpan = TimeSpan.FromSeconds(expUseTime);
            expStartTime = Time.time;
            if (task.IsFaulted)
            {
                Debug.Log("error while getting reference of data");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.HasChild(ExpName))
				{
                    string timeSpanValue = (string) snapshot.Child(ExpName).Value;
                    Debug.Log($" ++++ previous time of {ExpName} : {timeSpanValue}");
                    TimeSpan netTimeSpan = totalTimeSpan.Add(TimeSpan.Parse(timeSpanValue));
                    Debug.Log($" ++++ total net time of {ExpName}: {totalTimeSpan} + {timeSpanValue} = {netTimeSpan}");
                    database.GetReference(Exp).Child(ExpName).SetValueAsync(netTimeSpan.ToString());
                }
                else
				{
                    database.GetReference(Exp).Child(ExpName).SetValueAsync(totalTimeSpan.ToString());
                }
            }
        });
    }
}

public class senseExp
{
    public string ExpUseTime;

    public senseExp(string time)
	{
        this.ExpUseTime = time;
	}
}