using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;
using System;

public class Experiences_FDB : MonoBehaviour
{
    private DatabaseReference dbRef;
    private FirebaseDatabase database;

    private string headID = "SenseXR1";
    private string Exp = "MyExperiences";

    private Dictionary<string, string> expTimeMap = new();
    private bool faultTaskResult = false;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        database = FirebaseDatabase.GetInstance("https://fir-databasetest2-e7e86-default-rtdb.firebaseio.com/");
        dbRef = database.RootReference;
        //DatabaseReference ref = database.getReference("server/saving-data/fireblog");
        invokeExpStartListener();
    }

    public void invokeExpStartListener()
    {
        startTime = Time.time;
    }
    
    public void invokeExpEndListener(string ExpName)
    {
        float expUseTime = Time.time - startTime;
        TimeSpan totalTimeSpan = TimeSpan.FromSeconds(expUseTime);

        //expTimeMap.Add(ExpName, totalTimeSpan.ToString());
        //User3 user = new(expTimeMap);

        //string jsonString = JsonUtility.ToJson(user);
        //dbRef.Child(headID).Child(Exp).SetRawJsonValueAsync(jsonString);

        DatabaseReference expRef = dbRef.Child("SenseXR/Experiences");
        Dictionary<string, User3> myExperiences = new();
        myExperiences.Add("finestExperience",new User3(ExpName,totalTimeSpan.ToString()));
        myExperiences.Add("secondExperience",new User3(ExpName,totalTimeSpan.Add(TimeSpan.FromSeconds(360)).ToString()));

        expRef.SetValueAsync(myExperiences);
        //dbRef.Child(headID).Child(Exp).Child("expTimeMap").GetValueAsync().ContinueWith(task =>
        //{
        //    if (task.IsFaulted)
        //    {
        //        // Handle the error...
        //        faultTaskResult = true;
        //        Debug.Log("Error fetching data: " + task.Exception.ToString());
        //    }
        //    else if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;
        //        if (snapshot.Exists)
        //        {
        //            expTimeMap = snapshot.Value as Dictionary<string, string>;
        //
        //            // Modify existingDictionary 
        //            if (expTimeMap.ContainsKey(ExpName))
        //            {
        //                TimeSpan netTimeSpan = TimeSpan.Parse(expTimeMap[ExpName]).Add(totalTimeSpan);
        //                expTimeMap[ExpName] = (netTimeSpan.ToString());
        //            }
        //            else
        //            {
        //                expTimeMap.Add(ExpName, totalTimeSpan.ToString());
        //            }
        //
        //            // Save the modified dictionary back to the database
        //            dbRef.Child("expTimeMap").SetRawJsonValueAsync(JsonUtility.ToJson(expTimeMap));
        //        }
        //        else
        //        {
        //            //add map if it doesnot exist
        //            expTimeMap.Add(ExpName,totalTimeSpan.ToString());
        //            Debug.Log(expTimeMap.ContainsKey(ExpName)+ "+++++");
        //
        //            User3 user = new User3(expTimeMap);
        //
        //            string jsonString = JsonUtility.ToJson(user);
        //            dbRef.Child(headID).Child(Exp).SetRawJsonValueAsync(jsonString);
        //        }
        //    }
        //});
    }
}

public class User3
{
    public string expName;
    public string expUseTime;

    public User3(string _name, string _expUseTime)
	{
        this.expName = _name;
        this.expUseTime = _expUseTime;
	}
}