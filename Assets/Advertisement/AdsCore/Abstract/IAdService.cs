namespace Advertisement.AdsCore.Abstract {
    using Structures;
    using UniRx;

    public interface  IAdService {
        void Initialize();
        void MakeRequestForAd();
        void ShowAd();
        IReactiveProperty<AdStatus> Status { get; }
    }
}