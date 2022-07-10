using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    public string ownerID;
    public string horseName;
    public int age;
    //public static string horseID;

    public Horse(string ownerID, string horseName, int age)
    {
        this.ownerID = ownerID;
        this.horseName = horseName;
        this.age = age;
        //this.horseID = horseID;

    }
}

