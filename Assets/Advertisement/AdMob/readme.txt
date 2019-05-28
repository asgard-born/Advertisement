
Make sure that you already imported module AsdCore to your project

1. Make import AdMob module for your project
2. Create and setting up your own Configuration with Create -> AdMobConfiguration
3. Create the AdMobApi instance and put instance of this configuration to AdMobApi constructor inside your code
4. You can subscribe to the status by observing on reactive properties: BannerAdStatus, RewardedAdStatus and InterstitialAdStatus