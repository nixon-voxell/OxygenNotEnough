using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeliumUI : MonoBehaviour
{
    public GameObject Helium1,Helium2,Helium3;
    public void GetHelium(int helium_num)
    {
        if(helium_num == 0)
        {
            Helium1.GetComponent<Image>().color = new Color32(255,255,255,255);
            Helium2.GetComponent<Image>().color = new Color32(255,255,255,255);
            Helium3.GetComponent<Image>().color = new Color32(255,255,255,255);
        }
        else if(helium_num == 1)
        {
            Helium1.GetComponent<Image>().color = new Color32(255,0,0,255);
            Helium2.GetComponent<Image>().color = new Color32(255,255,255,255);
            Helium3.GetComponent<Image>().color = new Color32(255,255,255,255);
        }
        else if(helium_num == 2)
        {
            Helium1.GetComponent<Image>().color = new Color32(255,0,0,255);
            Helium2.GetComponent<Image>().color = new Color32(255,0,0,255);
            Helium3.GetComponent<Image>().color = new Color32(255,255,255,255);

        }
        else if(helium_num == 3)
        {
            Helium1.GetComponent<Image>().color = new Color32(255,0,0,255);
            Helium2.GetComponent<Image>().color = new Color32(255,0,0,255);
            Helium3.GetComponent<Image>().color = new Color32(255,0,0,255);
        }
    }
}
