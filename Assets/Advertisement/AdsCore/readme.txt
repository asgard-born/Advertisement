This is the advertising core for the custom implementations

1. Use ShowBannerAd, ShowInterstitialAd, ShowRewardedAd, HideBannerAd methods for the implementation
2. You can subscribe to the AdStatus

	LoadSuccess    -- when your ad has succesfully preload and ready for show
	LoadFailed     -- when your ad has problems with loading from remote server
	ShowStarted    -- when the video ad has succesfully started,
	ShowSuccess    -- f.e. if the Banner Ad has successfully shown
	Closed         -- when ad is closed by user
	ShowFailed     -- usually on this moment your add has already succesfully loaded
	Rewarded       -- use this after success finish of rewarded video Ad
	NotSupported   -- when current advertising doesn't support the current type of advertising
	NotImplemented -- when default value when callback is not ready