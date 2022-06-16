using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using Firebase.Extensions;

public class StablesScript : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public DatabaseReference DBreference;

    public TMP_Text userNameText, horsesText;
    public GameObject stablesScroll;
    public GameObject horseButton;
    public Transform stablesScrollTransform;

    FirebaseAuth auth;
    FirebaseUser user;


    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        LoadHorses();
        //delete this later
        userNameText.text = LoginScript.profileUserName;

    }

    private void LoadHorses()
    {
        FirebaseDatabase.DefaultInstance.GetReference("horses").GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
                Debug.LogWarning(message: $"Failed to register task with {task.Exception}");
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;


              //make suure to destroy all in transform


              foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
              {
                    string horseID = childSnapshot.Key;
                    string horseName = childSnapshot.Child("horseName").Value.ToString();
                    //string horseID = childSnapshot.Child().Value.ToString();
                    //int age = int.Parse(childSnapshot.Child("age").Value.ToString());
                    //string ownerID = childSnapshot.Child("ownerID").Value.ToString();

                    //Debug.Log(horseName);

                    GameObject q = Instantiate(horseButton, stablesScrollTransform);
                    q.GetComponent<HorseButtonClass>().NewHorseButton(horseName, horseID);

                }
            }
      });
    }

    //IEnumerator LoadzHorses()
    //{
    //    var DBTask = DBreference.Child("horses").OrderByChild("age").GetValueAsync();

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        DataSnapshot snapshot = DBTask.Result;

    //        foreach (Transform child in stablesScrollTransform.transform)
    //        {
    //            Destroy(child.gameObject);
    //        }

    //        //loop through every users UID
    //        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
    //        {

    //            string name = childSnapshot.Child("name").Value.ToString();
    //            int age = int.Parse(childSnapshot.Child("age").Value.ToString());
    //            string ownerID = childSnapshot.Child("ownerID").Value.ToString();

    //            Debug.Log(name);

    //            ////instantiate button
    //            GameObject stablesElement = Instantiate(horseButton, stablesScrollTransform);
    //            stablesElement.GetComponent<HorseButtonClass>().NewHorseButton(name);
    //            newHorseButton.transform.SetParent(scoreboardContent.transform, false);

    //            GameObject horseButton = Instantiate(horseButton, scoreboardContent);
    //            horseElement.GetComponent<HorseElement>().NewHorseElement(ownerID, name, age);


    //            ////instantiate new scoreboard elements
    //            //GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
    //            //scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(name);

    //            ////instantiate new scoreboard elements
    //            //GameObject stablesElement = Instantiate(horseElement, scoreboardContent);
    //            //stablesElement.GetComponent<HorseElement>().NewHorseElement(ownerID, name, age);
    //        }
    //    }
    //}


    public void LogOut()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
        Debug.Log("Signed out");

    }
}
