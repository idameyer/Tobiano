using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HPTabGroup : MonoBehaviour
{

    public GameObject tabButton1;
    public GameObject tabButton2;
    public GameObject tabButton3;
    public GameObject tabButton4;
    public GameObject tabButton5;
    public GameObject tabButton6;

    public GameObject tabContent1;
    public GameObject tabContent2;
    public GameObject tabContent3;
    public GameObject tabContent4;
    public GameObject tabContent5;
    public GameObject tabContent6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTabs()
    {
        tabContent1.SetActive(false);
        tabContent2.SetActive(false);
        tabContent3.SetActive(false);

    }

    public void TestClick()
    {
        Debug.Log("clicked a button");
    }

    public void ShowTab1()
    {
        ResetTabs();
        tabContent1.SetActive(true);
    }

    public void ShowTab2()
    {
        ResetTabs();
        tabContent2.SetActive(true);
    }

    public void ShowTab3()
    {
        ResetTabs();
        tabContent3.SetActive(true);
    }

    public void ShowTab4()
    {
        ResetTabs();
        tabContent4.SetActive(true);
    }

    public void ShowTab5()
    {
        ResetTabs();
        tabContent5.SetActive(true);
    }

    public void ShowTab6()
    {
        ResetTabs();
        tabContent6.SetActive(true);
    }

}
