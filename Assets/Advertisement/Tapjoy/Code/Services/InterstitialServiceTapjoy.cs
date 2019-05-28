namespace Advertisement.Tapjoy.Code.Services {
    using System;
    using AdsCore.Abstract;
    using AdsCore.Structures;
    using TapjoyUnity;
    using UniRx;

    public class InterstitialServiceTapjoy : IAdService {

        public  IReactiveProperty<AdStatus> Status => this.status;
        
        private IReactiveProperty<AdStatus> status = new ReactiveProperty<AdStatus>(
            new AdStatus(AdType.Interstitial, RequestStatus.NotImplemented, "Not implemented"));
        
        private TJPlacement placement;
        private IDisposable requestSubscriber;

        private string placementId;
        private int    reloadDelay;

        public InterstitialServiceTapjoy(string placementId, int reloadDelay) {
            this.placementId = placementId;
            this.reloadDelay = reloadDelay;
            this.Initialize();
        }
        
        public void Initialize() {
            this.placement = TJPlacement.CreatePlacement(this.placementId);
            this.MakeRequestForAd();

            TJPlacement.OnRequestFailure += this.OnRequestFailure;
            TJPlacement.OnContentReady   += this.OnContentReady;
            TJPlacement.OnContentDismiss += this.OnContentDismiss;
            TJPlacement.OnVideoStart     += this.OnVideoStart;
            TJPlacement.OnVideoComplete  += this.OnVideoComplete;
            TJPlacement.OnVideoError     += this.OnVideoError;
        }

        private void OnContentDismiss(TJPlacement placement) {
            this.MakeRequestForAd();
        }

        private void OnContentReady(TJPlacement placement) {
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.LoadSuccess, String.Empty);
        }

        private void OnRequestFailure(TJPlacement placement, string error) {
            this.requestSubscriber = Observable.Timer(TimeSpan.FromSeconds(this.reloadDelay))
                .Subscribe(_ => this.MakeRequestForAd());

            var errorMessage = $"Can't load the video interstitial. {error}";
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.LoadFailed, errorMessage);
        }

        public void MakeRequestForAd() {
            this.placement.RequestContent();
            this.requestSubscriber?.Dispose();
        }

        public void ShowAd() {
            this.placement.ShowContent();
        }
        
        private void OnVideoStart(TJPlacement tjPlacement) {
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.ShowStarted, String.Empty);
        }

        private void OnVideoComplete(TJPlacement tjPlacement) {
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.ShowSuccess, String.Empty);
            this.MakeRequestForAd();
        }

        private void OnVideoError(TJPlacement tjPlacement, string errorMessage) {
            this.status.Value = new AdStatus(AdType.Interstitial, RequestStatus.ShowFailed, errorMessage);
            this.MakeRequestForAd();
        }
    }
}