using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class LoadSceneManager : MMPersistentSingleton<LoadSceneManager>
{
    [SerializeField] private MMF_Player mmPlayer;
    private MMF_LoadScene mmLoadScene;

    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
        mmLoadScene = mmPlayer.GetFeedbackOfType<MMF_LoadScene>();
    }
    public void LoadScene(string nameScene)
    {
        mmLoadScene.DestinationSceneName = nameScene;
        mmPlayer.PlayFeedbacks();
    }

    private IEnumerator Start()
    {
        //LevelManager.LoadDataCacheNext();
        yield return new WaitForSecondsRealtime(2);
        LoadScene("InGameScene");
    }
}