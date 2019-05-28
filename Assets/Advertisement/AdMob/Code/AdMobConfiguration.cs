namespace Advertisement.AdMob.Code {
    using System;
    using AdsCore.Structures;
    using GoogleMobileAds.Api;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(AdMobConfiguration), menuName = "AdMobConfiguration"), Serializable]
    public class AdMobConfiguration : ScriptableObject {
        public string AppId          = "ca-app-pub-3940256099942544~3347511713";
        public string BannerId       = "ca-app-pub-3940256099942544/6300978111";
        public string RewardId       = "ca-app-pub-3940256099942544/5224354917";
        public string InterstitialId = "ca-app-pub-3940256099942544/1033173712";
        public int    ReloadDelay    = 10;

        [Space]
        [Header("Choose the banner size")]
        [SerializeField]
        private BannerType bannerType;
        [Header("Or set up your own size")]
        [SerializeField]
        private CustomBannerSize customBannerSize;
        [Space] public AdPosition BannerPosition;

        public  AdSize AdBannerSize => this.adBannerSize;
        private AdSize adBannerSize;

        private void OnEnable() => this.adBannerSize = this.CreateBannerAdSize();

        private AdSize CreateBannerAdSize() {
            AdSize adSize;

            if (this.customBannerSize.Width == 0 || this.customBannerSize.Height == 0) {
                switch (this.bannerType) {
                    case BannerType.BANNER:
                        adSize = AdSize.Banner;
                        break;
                    case BannerType.MEDIUM_RECTANGLE:
                        adSize = AdSize.MediumRectangle;
                        break;
                    case BannerType.FULL_BANNER:
                        adSize = AdSize.IABBanner;
                        break;
                    case BannerType.LEADERBOARD:
                        adSize = AdSize.Leaderboard;
                        break;
                    case BannerType.SMART_BANNER:
                        adSize = AdSize.SmartBanner;
                        break;
                    default:
                        adSize = AdSize.Banner;
                        break;
                }
            }
            else {
                adSize = new AdSize(this.customBannerSize.Width, this.customBannerSize.Height);
            }

            return adSize;
        }
    }
}