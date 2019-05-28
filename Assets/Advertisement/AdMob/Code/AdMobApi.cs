namespace Advertisement.AdMob.Code {
    using AdsCore.Abstract;
    using AdsCore.Structures;
    using GoogleMobileAds.Api;
    using LunarConsolePlugin;
    using Services;
    using UniRx;
    using UnityEngine;

    public class AdMobApi : IAdApi {
        public IReadOnlyReactiveProperty<AdStatus> BannerAdStatus {
            get {
                if (this.bannerServiceAdMob == null) {
                    this.bannerAdStatus.Value = new AdStatus(AdType.Rewarded, RequestStatus.NotImplemented, "Not Implemented");
                }

                return this.bannerAdStatus;
            }
        }

        public IReadOnlyReactiveProperty<AdStatus> RewardedAdStatus {
            get {
                if (this.rewardedServiceAdMob == null) {
                    this.rewardedAdStatus.Value = new AdStatus(AdType.Rewarded, RequestStatus.NotImplemented, "Not Implemented");
                }

                return this.rewardedAdStatus;
            }
        }

        public IReadOnlyReactiveProperty<AdStatus> InterstitialAdStatus {
            get {
                if (this.interstitialServiceAdMob == null) {
                    this.interstitialAdStatus.Value = new AdStatus(AdType.Interstitial, RequestStatus.NotImplemented, "Not Implemented");
                }

                return this.interstitialAdStatus;
            }
        }

        private AdMobConfiguration configuration;

        private IBannerService bannerServiceAdMob;
        private IAdService     rewardedServiceAdMob;
        private IAdService     interstitialServiceAdMob;

        private ReactiveProperty<AdStatus> bannerAdStatus       = new ReactiveProperty<AdStatus>();
        private ReactiveProperty<AdStatus> rewardedAdStatus     = new ReactiveProperty<AdStatus>();
        private ReactiveProperty<AdStatus> interstitialAdStatus = new ReactiveProperty<AdStatus>();

        public AdMobApi(AdMobConfiguration configuration) {
            this.configuration = configuration;
            MobileAds.Initialize(this.configuration.AppId);

            this.bannerServiceAdMob       = new BannerServiceAdMob(this.configuration.BannerPosition, this.configuration.AdBannerSize, this.configuration.BannerId, this.configuration.ReloadDelay);
            this.rewardedServiceAdMob     = new RewardedServiceAdMob(this.configuration.RewardId, this.configuration.ReloadDelay);
            this.interstitialServiceAdMob = new InterstitialServiceAdMob(this.configuration.InterstitialId, this.configuration.ReloadDelay);

            this.bannerServiceAdMob.Status
                .Subscribe(value => this.bannerAdStatus.Value = value);

            this.rewardedServiceAdMob.Status
                .Subscribe(value => this.rewardedAdStatus.Value = value);

            this.interstitialServiceAdMob.Status
                .Subscribe(value => this.interstitialAdStatus.Value = value);
        }

        public void ShowBannerAd()       => this.bannerServiceAdMob.ShowAd();
        public void HideBannerAd()       => this.bannerServiceAdMob.HideAd();
        public void ShowRewardedAd()     => this.rewardedServiceAdMob.ShowAd();
        public void ShowInterstitialAd() => this.interstitialServiceAdMob.ShowAd();
    }
}