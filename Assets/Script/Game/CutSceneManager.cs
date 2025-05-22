using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private VideoPlayer videoPlayer;

    private bool isFinished = false;

    public bool HasCutsceneForDay(int day)
    {
        return day == 0; //|| day == 6 || day == 12 || day == 18 || day == 24;
    }

    public IEnumerator PlayCutscene(int day)
    {
        cutscenePanel.SetActive(true);
        videoPlayer.Stop(); 

        isFinished = false;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        while (!isFinished)
        {
            yield return null;
        }

        cutscenePanel.SetActive(false);

        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        isFinished = true;
    }
}
