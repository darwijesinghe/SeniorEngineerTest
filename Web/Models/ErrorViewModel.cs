namespace Web.Models
{
    /// <summary>
    /// Model for the error page
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// The current operation ID
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Shows the request ID in view page
        /// </summary>
        public bool ShowRequestId
        {
            get
            {
                return !string.IsNullOrEmpty(RequestId);
            }
        }
    }
}
