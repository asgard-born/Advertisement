namespace Advertisement.Tapjoy.Code.Example {
    using AdsCore.Structures;
    using Code;
    using LunarConsolePlugin;
    using UI;
    using UniRx;
    using UnityEngine;

    public class TapjoyClient : MonoBehaviour {
        [SerializeField] private TapjoyConfiguration configuration;

        private TapjoyApi    api;
        private TapjoyWindow window;
        private string       windowLoadPath = "TapjoyWindow";

        private void Start() {
            this.api = new TapjoyApi(this.configuration);
            this.RegisterStatusObservation();
            this.RegisterLunar();
            this.LoadWindow();
        }

        private void RegisterStatusObservation() {
            this.api.RewardedAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status == RequestStatus.LoadSuccess)
                .Subscribe(_ => Debug.Log("Rewarded Load Success"));

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

            this.api.RewardedAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status == RequestStatus.Rewarded)
                .Subscribe(_ => Debug.Log("Rewarded Show Success"));

            this.api.InterstitialAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status == RequestStatus.LoadSuccess)
                .Subscribe(_ => Debug.Log("Interstitial Load Success"));

            this.api.InterstitialAdStatus
                .ObserveOnMainThread()
                .Where(value => value.Status == RequestStatus.ShowSuccess)
                .Subscribe(_ => Debug.Log("Interstitial Show Success"));
        }

        private void RegisterLunar() {
            LunarConsole.RegisterAction("Show UI", this.ShowUI);
            LunarConsole.RegisterAction("Hide UI", this.HideUI);
        }

        private void LoadWindow() {
            var go = Object.Instantiate(Resources.Load(this.windowLoadPath) as GameObject);
            this.window = go.GetComponent<TapjoyWindow>();

            this.window.ErrorPanel.SetActive(false);
            this.window.CloseErrorPanelBtn.OnClickAsObservable()
                .Subscribe(_ => this.window.ErrorPanel.SetActive(false));

            this.window.RewardButton.OnClickAsObservable().Subscribe(_ => this.api.ShowRewardedAd());
            this.window.InterstitialButton.OnClickAsObservable().Subscribe(_ => this.api.ShowInterstitialAd());
        }

        private void ShowUI() => this.window.gameObject.SetActive(true);

        private void HideUI() => this.window.gameObject.SetActive(false);
    }
}