using System.Collections;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject cutscenePanel;

    public bool HasCutsceneForDay(int day)
    {
        return day == 5 || day == 10;
    }

    public IEnumerator PlayCutscene(int day)
    {
        cutscenePanel.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        cutscenePanel.SetActive(false);
    }
}