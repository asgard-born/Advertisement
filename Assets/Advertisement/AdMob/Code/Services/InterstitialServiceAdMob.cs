namespace Advertisement.AdMob.Code.Services {
    using System;
    using AdsCore.Abstract;
    using AdsCore.Structures;
    using GoogleMobileAds.Api;
    using UniRx;

    public class InterstitialServiceAdMob : IAdService {
        public IReactiveProperty<AdStatus> Status => this.status;

        private IReactiveProperty<AdStatus> status = new ReactiveProperty<AdStatus>(
            new AdStatus(AdType.Interstitial, RequestStatus.NotImplemented, "Not implemented"));

        private InterstitialAd interstitialAd;
        private IDisposable    interstitialSubscriber;

        private string interstitialId;
        private int    reloadDelay;

        public InterstitialServiceAdMob(string interstitialId, int reloadDelay) {
            this.interstitialId = interstitialId;
            this.reloadDelay    = reloadDelay;

            this.Initialize();
        }

        public void Initialize() {
            this.interstitialAd = new InterstitialAd(this.interstitialId);
            this.MakeRequestForAd();

            this.interstitialAd.OnAdLoaded       += this.OnAdLoaded;
            this.interstitialAd.OnAdFailedToLoad += this.OnAdFailedToLoad;
            this.interstitialAd.OnAdOpening      += this.OnAdOpened;
            this.interstitialAd.OnAdClosed       += this.OnAdClosed;
        }
        
        public void MakeRequestForAd() {
            this.interstitialAd.LoadAd(new AdRequest.Builder().Build());
            this.interstitialSubscriber?.Dispose();
        }
        
        public void ShowAd() => this.interstitialAd.Show();

        public void OnAdLoaded(object sender, EventArgs args) {
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.LoadSuccess, String.Empty);
        }

        public void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
            this.interstitialSubscriber = Observable.Timer(TimeSpan.FromSeconds(this.reloadDelay))
                .Subscribe(_ => this.MakeRequestForAd());

            var errorMessage = $"Can't show the interstitial video. {args.Message}";
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.LoadFailed, errorMessage);
        }

        public void OnAdOpened(object sender, EventArgs args) {
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.ShowSuccess, String.Empty);
        }
        
        public void OnAdClosed(object sender, EventArgs e) {
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.Closed, String.Empty);
            this.MakeRequestForAd();
        }
    }
}