using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    [SerializeField] private string appId;
    [SerializeField] private string adBannerID;
    [SerializeField] private string adInterstitialId;
    [SerializeField] private AdPosition bannerPosition;
    [SerializeField] private bool testDevice = false;

    private BannerView bannerView;
    private InterstitialAd interstitial;


    public static AdManager Instance { get; private set; }

    public static Action OnInterstitialAdsClosed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance=this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (testDevice)
        {
            List<string> testDevices = new List<string>();
            testDevices.Add(SystemInfo.deviceUniqueIdentifier);

            RequestConfiguration requestConfiguration =
                new RequestConfiguration.Builder()
                    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
                    .SetTestDeviceIds(testDevices).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }

        MobileAds.Initialize(initStatus => { });

        CreateBanner(CreateRequest());
        CreateInterstitialAD(CreateRequest());

        interstitial.OnAdClosed+=InterstitialAdClosed;
    }

    private void OnDisable()
    {
        if(interstitial!=null)
            interstitial.OnAdClosed-=InterstitialAdClosed;
    }

    private void InterstitialAdClosed(object sender, EventArgs e)
    {
        OnInterstitialAdsClosed?.Invoke();
    }

    private AdRequest CreateRequest()
    {
        var request = new AdRequest.Builder().Build();

        return request;
    }

    public void CreateInterstitialAD(AdRequest adRequest)
    {
        if(interstitial!=null)
            interstitial.Destroy();

        interstitial=new InterstitialAd(adInterstitialId);
        interstitial.LoadAd(adRequest);
    }

    public void ShowInterstitialAd()
    {
        // if add is available
        /*if(interstitial.IsLoaded() && interstitial!=null)
            interstitial.Show();

        // if not available
        interstitial.LoadAd(CreateRequest());*/
    }

    public void CreateBanner(AdRequest request)
    {
        if(bannerView != null)
            bannerView.Destroy();

        bannerView=new BannerView(adBannerID,AdSize.SmartBanner,bannerPosition);
        bannerView.LoadAd(request);
        HideBanner();
    }

    public void HideBanner()
    {
        bannerView.Hide();
    }

    public void ShowBanner()
    {
        bannerView.Show();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        print("Application Paused");
    }
}
