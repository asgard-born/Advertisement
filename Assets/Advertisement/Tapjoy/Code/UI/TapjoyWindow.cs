namespace Advertisement.Tapjoy.Code.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public class TapjoyWindow : MonoBehaviour {
        public Button RewardButton;
        public Button InterstitialButton;

        [Space]
        public GameObject ErrorPanel;
        public Text       ErrorText;
        public Button     CloseErrorPanelBtn;
    }
}