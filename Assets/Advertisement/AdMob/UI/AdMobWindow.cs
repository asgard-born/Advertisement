namespace Advertisement.AdMob.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public class AdMobWindow : MonoBehaviour {
        public Button ShowBannerBtn;
        public Button HideBannerBtn;
        public Button ShowRewardBtn;
        public Button ShowInterstitialBtn;

        [Space]
        public GameObject ErrorPanel;
        public Text       ErrorText;
        public Button     CloseErrorPanelBtn;
    }
}