using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class CutsceneEntry
{
    public int day;               // 몇 일차 컷씬인지
    public VideoClip videoClip;  // 재생할 영상
}
public class CutSceneManager : MonoBehaviour
{

    public static CutSceneManager Instance;

    [SerializeField] private GameObject cutscenePanel;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private List<CutsceneEntry> cutsceneEntry = new List<CutsceneEntry>();

    private bool isFinished;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 해당 day에 컷씬이 등록되어 있는지 확인
    /// </summary>
    public bool HasCutsceneForDay(int day)
    {
        return cutsceneEntry.Exists(e => e.day == day && e.videoClip != null);
    }

    /// <summary>
    /// 해당 day의 컷씬 실행
    /// </summary>
    public IEnumerator PlayCutscene(int day)
    {
        var entry = cutsceneEntry.Find(e => e.day == day && e.videoClip != null);
        if (entry == null) yield break;

        cutscenePanel.SetActive(true);
        isFinished = false;

        videoPlayer.clip = entry.videoClip;
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