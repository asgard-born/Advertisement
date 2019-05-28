namespace Advertisement.AdsCore.Abstract {
    using Structures;
    using UniRx;

    public interface IAdApi {
        IReadOnlyReactiveProperty<AdStatus> BannerAdStatus { get; }
        IReadOnlyReactiveProperty<AdStatus> RewardedAdStatus { get; }
        IReadOnlyReactiveProperty<AdStatus> InterstitialAdStatus { get; }
        
        void ShowBannerAd();
        void HideBannerAd();
        void ShowRewardedAd();
        void ShowInterstitialAd();
    }
}