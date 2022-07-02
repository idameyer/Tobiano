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

public class HorseProfileScript : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public DatabaseReference DBreference;

    public TMP_Text horseNameText;

    FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        //horseNameText.text = LoginScript.profileUserName;
        horseNameText.text = HorseButtonClass.horseID;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
