﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
public class API : MonoBehaviour
{
    private static API instance = null;
    public static API Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new API();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://arboxingfb.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //User temp = getUserById(4);
        //print("NAME: " + temp.userName + "SCORE: " + temp.score + "ID: " + temp.id);
    }

    public DatabaseReference reference { get; set; } 

    // Update is called once per frame
    void Update()
    {
        
    }
    void Reset()
    {
        Debug.Log("EXcute reset!");
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://arboxingfb.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void addNewUser(string userName, double score, double id)
    {
        print("addUser!");
        
        DateTime dt2 = new DateTime();
        dt2 = DateTime.Now;

        String dateString = dt2.ToString("yyyy-MM-dd HH:mm:ss");

        User user = new User(userName, score, id, dateString);
        string json = JsonUtility.ToJson(user);

        print(reference);
       
        reference.Child("Users").Child(id.ToString()).SetRawJsonValueAsync(json);
    }

    public void getUser(UnityAction<User[]> callback)
    {
        print("this is make");

        FirebaseDatabase.DefaultInstance.GetReference("Users")
            .GetValueAsync().ContinueWith(task =>
        {
            List<User> users = new List<User>();
            if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;

                print("the number of count is " + snapShot.ChildrenCount);

                foreach (DataSnapshot data in snapShot.Children)
                {
                    User MyUser = new User();
                    IDictionary user = (IDictionary)data.Value;
                    //Debug.Log("NAME: " + user["userName"] + "SCORE: " + user["score"] + "ID: " + user["id"]);
                    print("NAME: " + user["userName"] + "SCORE: " + user["score"] + "ID: " + user["id"] + "DATE: " + user["dateString"]);

                    string name = "" + user["userName"];

                    MyUser.userName = "" + user["userName"];
                    MyUser.score = Convert.ToInt32("" + user["score"]);
                    MyUser.id = Convert.ToInt32("" + user["id"]);
                    MyUser.dateString = "" + user["dateString"];
                    
                    users.Add(MyUser);
                }
                callback.Invoke(users.ToArray());

            } else
            {
                Debug.Log("false");
                print("false");
            }
        });
    }

    //이것은 쓰지 않는함수라 업데이트하지 않았음
    public User getUserById(double id)
    {
        User MyUser = new User();

        FirebaseDatabase.DefaultInstance.GetReference("Users")
            .OrderByChild("id").EqualTo(id).GetValueAsync().ContinueWith(task =>
        {
            
            if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
   
                print("the number of count is " + snapShot.ChildrenCount);

                foreach(DataSnapshot data in snapShot.Children)
                {

                    IDictionary user = (IDictionary)data.Value;
                    //Debug.Log("NAME: " + user["userName"] + "SCORE: " + user["score"] + "ID: " + user["id"]);
                    print("NAME: " + user["userName"] + "SCORE: " + user["score"] + "ID: " + user["id"]);

                    string name = "" + user["userName"];

                    MyUser.userName = "" + user["userName"];
                    MyUser.score = Convert.ToInt32("" + user["score"]);
                    MyUser.id = Convert.ToInt32("" + user["id"]);
                }

            } else
            {
                Debug.Log("false");
                print("false");
            }
            
        });

        return MyUser;
    }
}
[Serializable]
public class User
{
    public string userName;
    public double score;
    public double id;
    public string dateString;

    public User()
    {
        this.userName = "NoName";
        this.score = 0;
        this.id = 0;
        this.dateString = "2020-11-16 12:00:00";
    }

    public User(string userName, double score, double id, string dateString)
    {
        this.userName = userName;
        this.score = score;
        this.id = id;
        this.dateString = dateString;
    }
}