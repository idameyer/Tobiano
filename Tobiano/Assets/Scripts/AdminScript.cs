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

public class AdminScript : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public DatabaseReference DBreference;

    public TMP_Text nameText, horseAge, ownerID, horseID;

    FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        //nameText.text = HorseButtonClass.horseID;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
