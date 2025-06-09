using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private VideoPlayer videoPlayer;

    private bool isFinished;

    public bool HasCutsceneForDay(int day)
    {
        // ������ �ƾ��� �ʿ��� ���� ���⿡ �߰�
        return day == 1;
    }

    public IEnumerator PlayCutscene(int day)
    {
        cutscenePanel.SetActive(true);
        isFinished = false;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        while (!isFinished)
            yield return null;

        cutscenePanel.SetActive(false);
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        isFinished = true;
    }
}
