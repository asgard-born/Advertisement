
Make sure that you already imported module AsdCore to your project

1. Make import Tapjoy module for your project
2. Setting up the SDK from Window -> Tapjoy
	2.1 Make sure you have GameObject on your scene with the 'Tapjoy Component' and 'Tapjoy Unity Init' components
3. Create and setting up your own Configuration with Create -> TapjoyConfiguration
4. Create the TapjoyApi instance and put instance of this configuration to TapjoyApi constructor inside your code
5. You can subscribe to the status by observing on reactive properties RewardedAdStatus and InterstitialAdStatus

The Tapjoy doesn't support Banner Ad