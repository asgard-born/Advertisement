namespace Advertisement.AdMob.Code.Services {
    using System;
    using AdsCore.Abstract;
    using AdsCore.Structures;
    using GoogleMobileAds.Api;
    using UniRx;

    public class RewardedServiceAdMob : IAdService {
        public IReactiveProperty<AdStatus> Status => this.status;

        private IReactiveProperty<AdStatus> status = new ReactiveProperty<AdStatus>(
            new AdStatus(AdType.Rewarded, RequestStatus.NotImplemented, "Not implemented"));

        private RewardBasedVideoAd videoAd;
        private IDisposable        reloadSubscriber;

        private string rewardedId;
        private int    reloadDelay;

        public RewardedServiceAdMob(string rewardedId, int reloadDelay) {
            this.rewardedId    = rewardedId;
            this.reloadDelay = reloadDelay;
            this.Initialize();
        }

        public void Initialize() {
            this.videoAd = RewardBasedVideoAd.Instance;
            this.MakeRequestForAd();

            this.videoAd.OnAdLoaded       += this.OnAdLoaded;
            this.videoAd.OnAdFailedToLoad += this.OnAdFailedToLoad;
            this.videoAd.OnAdOpening      += this.OnAdOpened;
            this.videoAd.OnAdClosed       += this.OnAdClosed;
            this.videoAd.OnAdRewarded     += this.OnAdRewarded;
        }
        
        public void MakeRequestForAd() {
            this.videoAd.LoadAd(new AdRequest.Builder().Build(), this.rewardedId);
            this.reloadSubscriber?.Dispose();
        }
        
        public void ShowAd() => this.videoAd.Show();

        public void OnAdLoaded(object sender, EventArgs args) {
            this.status.Value = new AdStatus(AdType.Rewarded, RequestStatus.LoadSuccess, String.Empty);
        }

        public void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
            this.reloadSubscriber = Observable.Timer(TimeSpan.FromSeconds(this.reloadDelay))
                .Subscribe(_ => this.MakeRequestForAd());

            var errorMessage = $"Can't load the video reward. {args.Message}";
            this.status.Value = new AdStatus(AdType.Rewarded, RequestStatus.LoadFailed, errorMessage);
        }
        
        public void OnAdOpened(object sender, EventArgs args) {
            this.status.Value = new AdStatus(AdType.Rewarded, RequestStatus.ShowSuccess, String.Empty);
        }
        
        public void OnAdClosed(object sender, EventArgs args) {
            this.status.Value = new AdStatus(AdType.Rewarded, RequestStatus.Closed, String.Empty);
            this.MakeRequestForAd();
        }

        public void OnAdRewarded(object sender, Reward args) {
            this.status.Value = new AdStatus(AdType.Rewarded, RequestStatus.Rewarded, String.Empty);
            this.MakeRequestForAd();
        }

    }
}