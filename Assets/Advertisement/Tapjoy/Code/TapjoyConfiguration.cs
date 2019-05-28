namespace Advertisement.Tapjoy.Code {
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(TapjoyConfiguration), menuName = "TapjoyConfiguration"), Serializable]
    public class TapjoyConfiguration : ScriptableObject {
        public string SdkKey                  = "3EBf-KZ9SbWrXzy6wdDBMAECCn5r4rbBG4nlOguCNqoA_q74e28cm9LTj3od";
        public string RewardedPlacementId     = "Rewarded";
        public string InterstitialPlacementId = "Interstitial";
        public int    ReloadDelay             = 10;
    }
}