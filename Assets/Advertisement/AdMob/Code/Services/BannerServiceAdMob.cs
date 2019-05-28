namespace Advertisement.AdMob.Code.Services {
    using System;
    using AdsCore.Abstract;
    using AdsCore.Structures;
    using GoogleMobileAds.Api;
    using UniRx;

    public class BannerServiceAdMob : IBannerService {
        public IReactiveProperty<AdStatus> Status => this.status;

        private IReactiveProperty<AdStatus> status = new ReactiveProperty<AdStatus>(
            new AdStatus(AdType.Rewarded, RequestStatus.NotImplemented, "Not implemented"));

        private BannerView  bannerView;
        private IDisposable bannerSubscriber;

        private AdPosition bannerPosition;
        private AdSize     bannerSize;
        private string     bannerId;
        private int        reloadDelay;

        public BannerServiceAdMob(AdPosition bannerPosition, AdSize bannerSize, string bannerId, int reloadDelay) {
            this.bannerPosition = bannerPosition;
            this.bannerSize     = bannerSize;
            this.bannerId       = bannerId;
            this.reloadDelay    = reloadDelay;

            this.Initialize();
        }

        public void Initialize() {
            this.bannerView = new BannerView(this.bannerId, this.bannerSize, this.bannerPosition);
            this.MakeRequestForAd();

            this.bannerView.OnAdLoaded       += this.OnAdLoaded;
            this.bannerView.OnAdFailedToLoad += this.OnAdFailedToLoad;
            this.bannerView.OnAdOpening      += this.OnAdOpened;
            this.bannerView.OnAdClosed       += this.OnAdClosed;
        }

        public void MakeRequestForAd() {
            this.bannerView.LoadAd(new AdRequest.Builder().Build());
            this.bannerSubscriber?.Dispose();
        }

        public void ShowAd() {
            this.bannerView.Show();
        }

        public void HideAd() {
            this.bannerView.Hide();
        }

        public void OnAdLoaded(object sender, EventArgs args) {
            this.bannerView.Hide();
            this.status.Value = new AdStatus(AdType.Banner, RequestStatus.LoadSuccess, String.Empty);
        }

        public void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
            this.bannerSubscriber = Observable.Timer(TimeSpan.FromSeconds(this.reloadDelay))
                .Subscribe(_ => this.MakeRequestForAd());

            var errorMessage = $"Can't load Banner. {args.Message}";
            this.status.Value = new AdStatus(AdType.Banner, RequestStatus.LoadFailed, errorMessage);
        }

        public void OnAdOpened(object sender, EventArgs args) {
            this.status.Value = new AdStatus(AdType.Banner, RequestStatus.ShowSuccess, String.Empty);
        }

        public void OnAdClosed(object sender, EventArgs args) {
            this.status.Value = new AdStatus(AdType.Banner, RequestStatus.Closed, String.Empty);
        }
    }
}