namespace Advertisement.AdsCore.Structures {
    public enum RequestStatus : byte {
        LoadSuccess    = 0,
        LoadFailed     = 10,
        ShowStarted    = 20,
        ShowSuccess    = 30,
        Closed         = 40,
        ShowFailed     = 50,
        Rewarded       = 60,
        NotSupported   = 70,
        NotImplemented = 80
    }
}