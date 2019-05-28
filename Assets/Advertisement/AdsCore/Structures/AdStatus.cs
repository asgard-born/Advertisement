namespace Advertisement.AdsCore.Structures {
    public struct AdStatus {
        public AdType AdType;
        public RequestStatus Status;
        public string ErrorMessage;

        public AdStatus(AdType adType, RequestStatus status, string errorMessage) {
            this.AdType = adType;
            this.Status = status;
            this.ErrorMessage = errorMessage;
        }
    }
}