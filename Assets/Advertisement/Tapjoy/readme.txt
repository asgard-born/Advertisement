
Make sure that you already imported module AsdCore to your project

1. Make import Tapjoy module for your project
2. Unpack the TapjoySettings package
3. Setting up the SDK from Window -> Tapjoy
	3.1 Make sure you have GameObject on your scene with the 'Tapjoy Component' and 'Tapjoy Unity Init' components
4. Create and setting up your own Configuration with Create -> TapjoyConfiguration
5. Create the TapjoyApi instance and put instance of this configuration to TapjoyApi constructor inside your code
6. You can subscribe to the status by observing on reactive properties RewardedAdStatus and InterstitialAdStatus

The Tapjoy doesn't support Banner Ad