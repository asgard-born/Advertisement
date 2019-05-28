namespace Advertisement.AdMob.Code.Example {
    using AdsCore.Abstract;
    using AdsCore.Structures;
    using LunarConsolePlugin;
    using UI;
    using UniRx;
    using UnityEngine;

    public class AdMobClient : MonoBehaviour {
        [SerializeField] private AdMobConfiguration configuration;

        private IAdApi      api;
        private AdMobWindow window;
        private string      windowLoadPath = "AdMobWindow";

        private void Start() {
            this.api = new AdMobApi(this.configuration);
            this.RegisterStatusObservation();
            this.RegisterLunar();
            this.LoadWindow();
        }

        private void RegisterStatusObservation() {

            this.api.RewardedAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status == RequestStatus.Rewarded)
                .Subscribe(_ => Debug.Log("Rewarded show success!"));

            this.api.InterstitialAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status == RequestStatus.ShowSuccess)
                .Subscribe(_ => Debug.Log("Interstitial show success!"));

            this.api.BannerAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status != RequestStatus.NotImplemented)
                .Do(value => {
                    if (value.Status == RequestStatus.ShowFailed || value.Status == RequestStatus.LoadFailed || value.Status == RequestStatus.NotSupported) {
                        this.window.ErrorPanel.SetActive(true);
                        this.window.ErrorText.text = value.ErrorMessage;
                    }
                })
                .Subscribe();
            
            this.api.InterstitialAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status != RequestStatus.NotImplemented)
                .Do(value => {
                    if (value.Status == RequestStatus.ShowFailed || value.Status == RequestStatus.LoadFailed || value.Status == RequestStatus.NotSupported) {
                        this.window.ErrorPanel.SetActive(true);
                        this.window.ErrorText.text = value.ErrorMessage;
                    }
                })
                .Subscribe();
            
            this.api.RewardedAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status != RequestStatus.NotImplemented)
                .Do(value => {
                    if (value.Status == RequestStatus.ShowFailed || value.Status == RequestStatus.LoadFailed || value.Status == RequestStatus.NotSupported) {
                        this.window.ErrorPanel.SetActive(true);
                        this.window.ErrorText.text = value.ErrorMessage;
                    }
                })
                .Subscribe();
        }

        private void RegisterLunar() {
            LunarConsole.RegisterAction("Show UI", this.ShowUI);
            LunarConsole.RegisterAction("Hide UI", this.HideUI);
        }

        private void LoadWindow() {
            var go = Object.Instantiate(Resources.Load(this.windowLoadPath) as GameObject);
            this.window = go.GetComponent<AdMobWindow>();

            this.window.ErrorPanel.SetActive(false);
            this.window.CloseErrorPanelBtn.OnClickAsObservable().Subscribe(_ => this.window.ErrorPanel.SetActive(false));

            this.window.ShowBannerBtn.OnClickAsObservable().Subscribe(_ => this.api.ShowBannerAd());
            this.window.HideBannerBtn.OnClickAsObservable().Subscribe(_ => this.api.HideBannerAd());
            this.window.ShowRewardBtn.OnClickAsObservable().Subscribe(_ => this.api.ShowRewardedAd());
            this.window.ShowInterstitialBtn.OnClickAsObservable().Subscribe(_ => this.api.ShowInterstitialAd());
        }

        private void ShowUI() => this.window.gameObject.SetActive(true);

        private void HideUI() => this.window.gameObject.SetActive(false);
    }
}