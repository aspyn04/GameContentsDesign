using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance;

    public int cheese = 0;
    public int star = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCheese(int amount)
    {
        cheese += amount;
    }

    public void AddStar(int amount)
    {
        star += amount;
    }

    public void ResetResources()
    {
        cheese = 0;
        star = 0;
    }
}
