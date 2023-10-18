using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 public class Toggle : MonoBehaviour
 {
    public KeyCode key = KeyCode.K;
 
 
     // Use this for initialization
     void Start()
     {
 
     }
 
     // Update is called once per frame
     void Update()
     {
         if(Input.GetKeyDown(key))
         {
             GetComponent<Outlinetest>().enabled = !GetComponent<Outlinetest>().enabled;
         }
     }
 }
 