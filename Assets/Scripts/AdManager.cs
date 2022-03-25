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
            Instance=this as AdManager;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
        CreateBanner(CreateRequest());
        CreateInterstitialAD(CreateRequest());

        interstitial.OnAdClosed+=InterstitialAdClosed;
    }

    private void OnDisable()
    {
        interstitial.OnAdClosed-=InterstitialAdClosed;
    }

    private void InterstitialAdClosed(object sender, EventArgs e)
    {
        OnInterstitialAdsClosed?.Invoke();
    }

    private AdRequest CreateRequest()
    {
        AdRequest request;

        if (testDevice)
        {
            List<string> testDevices=new List<string>();
            testDevices.Add(SystemInfo.deviceUniqueIdentifier);

            RequestConfiguration requestConfiguration =
                new RequestConfiguration.Builder().SetTestDeviceIds(testDevices).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);

            request = new AdRequest.Builder().Build();
        }
        else
            request = new AdRequest.Builder().Build();

        return request;
    }

    public void CreateInterstitialAD(AdRequest adRequest)
    {
        interstitial=new InterstitialAd(adInterstitialId);
        interstitial.LoadAd(adRequest);
    }

    public void ShowInterstitialAd()
    {
        // if add is available
        if(interstitial.IsLoaded())
            interstitial.Show();

        // if not available
        interstitial.LoadAd(CreateRequest());
    }

    public void CreateBanner(AdRequest request)
    {
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
}
