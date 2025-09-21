namespace Cerberus.Settings
{
    public class CerberusSettings
    {
        public ESISettings ESISettings { get; set; } 
    }

    public class ESISettings
    {
        /// <summary>
        /// ESI client id.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// ESI client secret
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
