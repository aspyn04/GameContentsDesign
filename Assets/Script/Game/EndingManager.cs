using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class EndingManager : MonoBehaviour
{
    public void Ending()
    {
        int cheese = GoodsManager.Instance.totalCheese;
        int stars = GoodsManager.Instance.totalStar;

        if (cheese >= 3000)
        {
            Debug.Log("[EndingManager] Good ending triggered.");
            SceneManager.LoadScene("Good");
        }
        else
        {
            Debug.Log("[EndingManager] Bad ending triggered.");
            SceneManager.LoadScene("Bad");
        }
    }
}
