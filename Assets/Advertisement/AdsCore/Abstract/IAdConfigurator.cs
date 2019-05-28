namespace Advertisement.AdsCore.Abstract {
    public interface IAdConfigurator {
        IAdApi AdApi { get; }
        void   CreateApi();
    }
}