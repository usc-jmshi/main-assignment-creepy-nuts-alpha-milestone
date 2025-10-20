using System;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_uiScript : MonoBehaviour{

    public TextMeshProUGUI text;
    public int zoneInt;

    private void OnTriggerEnter(Collider other)
    {
        changeText(zoneInt);
    }

    private void OnTriggerExit(Collider other)
    {
        changeText(-1);
    }

    public void changeText(int value)
    {
        switch (value)
        {
            case 1:{
                    text.text = "Move with WASD";

                    break;
                }
                
            case 2:{
                    text.text = "Use SHIFT to dash";
                    break;
                }

           case 3:
                {
                    text.text = "Use Left Mouse Click to change colors";
                    break;
                } 
            
            case 4:
                {
                    text.text = "Platforms can be stood on even without the right color";
                    break;
                }
            case -1:
                {
                    text.text = "";
                    break;
                }
            default:
                {
                    throw new InvalidCastException("How did we even get here???? -tutUIScript");
                }
        }
    }

}
