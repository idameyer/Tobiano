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

public class ShopScript : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public DatabaseReference DBreference;

    public FirebaseUser User;
    private string userID;

    public TMP_Text nameText;

    FirebaseAuth auth;

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
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

    // Start is called before the first frame update
    void Start()
    {
        //nameText.text = HorseButtonClass.horseID;
        userID = auth.CurrentUser.UserId;


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateSalesHorse()
    {
        System.Random rnd = new System.Random();
        Horse newHorse = new Horse(userID, "New Horse", rnd.Next(1, 26));
        string json = JsonUtility.ToJson(newHorse);
        DBreference.Child("salesHorses").Push().SetRawJsonValueAsync(json);
    }
}
