using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsButton : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameID="3950970";
#endif
#if UNITY_ANDROID
    private string gameID = "3950971";
#endif

    Button adsButton;
    private string placementID = "rewardedVideo";

    private void Start()
    {
        adsButton = GetComponent<Button>();
        Advertisement.Initialize(gameID, false);
        if (adsButton) adsButton.onClick.AddListener(ShowAds);
    }

    public void ShowAds() => Advertisement.Show(placementID);

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Finished:
                Debug.Log("广告结束，奖励");
                FindObjectOfType<PlayerController>().Rebirth();
                break;
        }

    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("广告开始了");
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (Advertisement.IsReady(placementId))
            Debug.Log("广告准备好了");
    }
}
