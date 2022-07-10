using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update


    public static string currentUser;
    public static string currentHorse;


    void Start()
    {
        //userNameText.text = LoginScript.profileUserName;


        //this script controlls the UI and is always present

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StableButton()
    {
        SceneManager.LoadScene("Stables");
    }

    public void FarmButton()
    {
        SceneManager.LoadScene("Farm");
    }

    public void ShopButton()
    {
        SceneManager.LoadScene("Shop");
    }

}
