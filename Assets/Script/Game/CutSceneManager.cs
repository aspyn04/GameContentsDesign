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
        return day == 1; //|| day == 2 || day == 10;
    }

    public IEnumerator PlayCutscene(int day)
    {
        Debug.Log("�ƾ� Ȱ��ȭ��");
        cutscenePanel.SetActive(true);
        videoPlayer.Stop(); 

        isFinished = false;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing...");
            yield return null;
        }

        Debug.Log("Prepared. Duration: " + videoPlayer.length + " seconds");

        videoPlayer.Play();
        Debug.Log("Play ȣ���, isPlaying: " + videoPlayer.isPlaying);


        while (!isFinished)
        {
            yield return null;
        }

        Debug.Log("���� ��� �Ϸ�");
        cutscenePanel.SetActive(false);

        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        isFinished = true;
    }
}
