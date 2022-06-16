using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HorseButtonClass : MonoBehaviour
{
    public TMP_Text nameText, ageText;
    public static string horseID;

    public void OpenHorseProfile()
    {
        //UIManager.currentHorse = 
        SceneManager.LoadScene("HorseProfile");
    }

    public void NewHorseButton(string _name, string _horseID)
    {
        nameText.text = _name;
        horseID = _horseID;
    }
}
