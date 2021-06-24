namespace WebApp.SpecialExternalApiHelpers
{
    /// <summary>
    /// Struct for transporting GyazoResponses
    /// </summary>
    public struct GyazoResponse
    {
        /// <summary>
        /// Url of the picture if upload went Successfully
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Is true if request was successful
        /// </summary>
        public bool IsSuccessfulResponse { get; set; }
        /// <summary>
        /// Error messages is any occurred
        /// </summary>
        public string Message { get; set; }
    }
}