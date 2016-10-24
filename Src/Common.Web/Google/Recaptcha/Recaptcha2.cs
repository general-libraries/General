using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Common.Web.Google.Recaptcha
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// Code from: https://github.com/xantari/RecaptchaV2.NET
    /// </remarks>
    public sealed class Recaptcha2
    {
        private const string HTML_TEMPLATE = @"<div class=""g-recaptcha"" data-sitekey=""{0}""></div>";

        private string SecretKey { get; set; }
        public string SiteKey { get; private set; }
        public Uri BaseUrl { get; private set; }

        /// <summary>
        /// Default constructor loads the site key and secret key from configuration file. The AppSettings keys are GoogleRecaptchaSiteKey and GoogleRecaptchaSecretKey
        /// </summary>
        /// <remarks>
        /// 8/4/2015 - MRO: Initial Creation
        /// </remarks>
        public Recaptcha2()
            : this(Manager.Instance.GlobalSetting.Recaptcha2PublicKey, Manager.Instance.GlobalSetting.Recaptcha2PrivateKey)
        { }

        /// <summary>
        /// Recaptcha constructor with passed in site key and secret key
        /// </summary>
        /// <param name="siteKey">Google Recaptcha Site Key</param>
        /// <param name="secretKey">Google Recaptcha Secret Key</param>
        public Recaptcha2(string siteKey, string secretKey)
        {
            SiteKey = siteKey;
            SecretKey = secretKey;
            BaseUrl = Manager.Instance.GlobalSetting.Recaptcha2Url;
        }

        /// <summary>
        /// Gets the HTML element for the recaptcha to be injected onto the HTML page.
        /// </summary>
        /// <returns>The HTML element for the recaptcha.</returns>
        public string GetSecureTokenHTML()
        {
            return string.Format(HTML_TEMPLATE, SiteKey);
        }

        /// <summary>
        /// Validates a Recaptcha V2 response.
        /// </summary>
        /// <param name="recaptchaResponse">g-recaptcha-response form response variable (HttpContext.Current.Request.Form["g-recaptcha-response"])</param>
        /// <returns>RecaptchaValidationResult</returns>
        /// <remarks>
        /// 8/4/2015 - Matt Olson: Initial creation.
        /// </remarks>
        public async Task<RecaptchaValidationResult> ValidateAsync(string recaptchaResponse)
        {
            RecaptchaValidationResult result = new RecaptchaValidationResult();

            string urlParameters = string.Format("?secret={0}&response={1}", SecretKey, recaptchaResponse); //&remoteip={2} , GetClientIp()

            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseUrl; // new Uri("https://www.google.com/recaptcha/api/siteverify");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsync(urlParameters, new StringContent(string.Empty));

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<RecaptchaValidationResult>();
                }
            }

            return result;
        }

        private string GetClientIp()
        {
            // Look for a proxy address first
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
            {
                return string.Empty;
            }

            String _ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // If there is no proxy, get the standard remote address
            if (string.IsNullOrWhiteSpace(_ip) || _ip.ToLower() == "unknown")
            {
                _ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return _ip;
        }
    }

    public class RecaptchaValidationResult
    {
        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
        public string Session_ID { get; set; }
        public bool Success { get; set; }
        public long TS_MS { get; set; }
        public string HostName { get; set; }
    }
}
