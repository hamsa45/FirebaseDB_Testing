using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class RealTimeDB_FB : MonoBehaviour
{

    #region public fields
    public string dataBaseUrl = "https://sensexr-play-default-rtdb.firebaseio.com/";
	#endregion

	#region private fields
	private const string Exp = "Experiences";
    private float expStartTime;
    private FirebaseDatabase database;
	#endregion

	// Start is called before the first frame update
	void Start()
    {
        database = FirebaseDatabase.GetInstance(dataBaseUrl);
    }

    public void startExperience()
    {
        expStartTime = Time.time;
    }

    public void checkAndSetExperienceUseTime(String ExpName)
    {
        database.GetReference(Exp).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            ExpName = ExpName.ToLower();

            //calculate total exp use time
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
                    string timeSpanValue = (string)snapshot.Child(ExpName).Value;

                    string[] arr = timeSpanValue.Split(' ');

                    TimeSpan netTimeSpan = totalTimeSpan.Add(TimeSpan.Parse($"{arr[0]}:{arr[2]}:{arr[4]}"));

                    string timeFormat = $"{netTimeSpan.Hours} Hrs {netTimeSpan.Minutes} Mins {netTimeSpan.Seconds} Secs";
                    
                    database.GetReference(Exp).Child(ExpName).SetValueAsync(timeFormat);
                }
                else
                {
                    string timeFormat = $"{totalTimeSpan.Hours} Hrs {totalTimeSpan.Minutes} Mins {totalTimeSpan.Seconds} Secs";
                    database.GetReference(Exp).Child(ExpName).SetValueAsync(timeFormat);
                }
            }
        });
    }
}
