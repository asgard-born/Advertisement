namespace Advertisement.Tapjoy.Code {
    using AdsCore.Abstract;
    using AdsCore.Structures;
    using Services;
    using TapjoyUnity;
    using UniRx;
    using UnityEngine;

    public class TapjoyApi : IAdApi {
        public IReadOnlyReactiveProperty<AdStatus> BannerAdStatus => this.bannerAdStatus;

        public IReadOnlyReactiveProperty<AdStatus> RewardedAdStatus {
            get {
                if (this.rewardedServiceTapjoy == null) {
                    this.rewardedAdStatus.Value = new AdStatus(AdType.Rewarded, RequestStatus.NotImplemented, "Not Implemented");
                }

                return this.rewardedAdStatus;
            }
        }

        public IReadOnlyReactiveProperty<AdStatus> InterstitialAdStatus {
            get {
                if (this.interstitialServiceTapjoy == null) {
                    this.interstitialAdStatus.Value = new AdStatus(AdType.Interstitial, RequestStatus.NotImplemented, "Not Implemented");
                }

                return this.interstitialAdStatus;
            }
        }

        private IReactiveProperty<AdStatus> rewardedAdStatus     = new ReactiveProperty<AdStatus>();
        private IReactiveProperty<AdStatus> interstitialAdStatus = new ReactiveProperty<AdStatus>();

        private IAdService rewardedServiceTapjoy;
        private IAdService interstitialServiceTapjoy;

        private TapjoyConfiguration         configuration;
        private IReactiveProperty<AdStatus> bannerAdStatus = new ReactiveProperty<AdStatus>();

        public TapjoyApi(TapjoyConfiguration configuration) {
            this.configuration = configuration;

            if (!Tapjoy.IsConnected) {
                this.Connect();
                Tapjoy.OnConnectSuccess += this.OnConnectSuccess;
            }
            else {
                this.OnConnectSuccess();
            }
        }

        public void ShowBannerAd() => this.SetBannerStatusNotSupported();
        public void HideBannerAd() => this.SetBannerStatusNotSupported();

        public void ShowRewardedAd()     => this.rewardedServiceTapjoy?.ShowAd();
        public void ShowInterstitialAd() => this.interstitialServiceTapjoy?.ShowAd();

        private void OnConnectSuccess() {
            Debug.Log("ConnectSuccess");
            
            this.rewardedServiceTapjoy     = new RewardedServiceTapjoy(this.configuration.RewardedPlacementId, this.configuration.ReloadDelay);
            this.interstitialServiceTapjoy = new InterstitialServiceTapjoy(this.configuration.InterstitialPlacementId, this.configuration.ReloadDelay);

            this.rewardedServiceTapjoy.Status
                .Subscribe(value => this.rewardedAdStatus.Value = value);

            this.interstitialServiceTapjoy.Status
                .Subscribe(value => this.interstitialAdStatus.Value = value);
        }

        private void Connect() => Tapjoy.Connect(this.configuration.SdkKey);

        private void SetBannerStatusNotSupported() =>
            this.bannerAdStatus.Value = new AdStatus(AdType.Banner, RequestStatus.NotSupported, "Not supported");
    }
}