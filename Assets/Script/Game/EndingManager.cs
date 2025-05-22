using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndingManager : MonoBehaviour
{

    public GameObject goodEndingPanel;
    public GameObject normalEndingPanel;
    public GameObject badEndingPanel;

    public void Ending()
    {
        int cheese = ResourceManager.Instance.cheese;
        int stars = ResourceManager.Instance.star;

        if (cheese >= 10 && stars >= 10)
        {
            goodEndingPanel.SetActive(false);
        }

        else if (cheese < 10 && cheese >= 5)
        {
            normalEndingPanel.SetActive(false);
        }

        else
        {
            badEndingPanel.SetActive(false);    
        }
    }
}
