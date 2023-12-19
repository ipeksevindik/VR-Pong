using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneExampleScript : MonoBehaviour
{
    

    public int Counter = 0;
    public TextMeshPro Click_txt;


    public void IncreaseCounter()
    {
        Counter++;
        Click_txt.text = "you clicked " + Counter.ToString() + " times";
    }

    public void DecreaseCounter() 
    {
        Counter--;
        Click_txt.text = "you clicked " + Counter.ToString() + " times";
    }
    
    public void ResetCounter()
    {
        Counter = 0;
        Click_txt.text = "you clicked " + Counter.ToString() + " times";

    }

}
