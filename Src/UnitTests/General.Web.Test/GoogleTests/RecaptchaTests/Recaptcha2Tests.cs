using System;
using System.Threading.Tasks;
using General.Web.Google.Recaptcha;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace General.Web.Test.GoogleTests.RecaptchaTests
{
    [TestClass]
    public class Recaptcha2Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var recaptcha = new Recaptcha2();

            var actual = recaptcha.GetSecureTokenHTML();

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var recaptcha = new Recaptcha2();

            string recaptchaResponse = "03AHJ_VusuS71Y0XLyzwQTVh_inqWfUbJO_nK_SRvcsR0xfJ0T1pjq37mrwIS0crstvVA_8u36ffmZNuuUbONrS8ma6GUtM5eTaJDxs-rxO5KWCVCL_SKW9Rn4NPQWh4BH6IVvOXHoLtvaoAIp9ZUxihSkX2PNgDhJTVtNCdAvmsawevAg-FbQi-W_R-__5UeH2YqrXcjqFxDXciEyAKJXGxCzI_jknVbIWyhmTGXnd7-7_uk8u1O-aRUBi6Yb1x-qq9nRH16j1eEviXTtYZ5-qnUFueheHjk7sa7bXkIt2nxqitlH_xgDAf3rYSIMpsOfB6CAnNlBbWNiJnIU5XThERCTB4h9dI2IcuQGz4BkjoG-2A1_UjM7GCmniSh2He449pM_m1CBr1Y8pvYNvhBbA3X6ar30Ezfq7kVSGsCsPxGCzIvX6N-XuVxzjVwf9go3Nn9xEUJyZFng1AxwDrd-z6OnTsegWxTs3zHqHN3lpYVUngNRtCkkzj6hjntyVWHbWcJeV4HG2av-RrmsoLBdsuWjq8nqs61DlAKQoUawVqZW4EeiRFouL3wuS7hZu3T9SoahfUFJv7eHsiP1xqQjT4iecANhfQLF7rvEbrNoQVUMtsy0BMujZJPnW7lkZy5OC3lY7hEd0w4kCjaMj1xZW9htcf2vGXya0o1xviBc00S8fIzsxkMoyJo9ksiPQP4qAhm1f3NL2fsHfKKqvCiRKxLeD7E7zlcz_bCokjAphUFhVC4HarrYhazmXeB4-XBeWMG2wp8td2NzxFP1YIsMpl4-u6cx1dJ2cHDdbQkJepEcSub1XhYi4Z0PCJxHL_1uLDLxXCZfv-2ffcDXgZXnlCc4Gm1eHsicbLSoECfhN9_qRpjpz96gbdA9C6FfNHtg1xOkWbT5U8MpEaZbbi-xG2lEfcCH0cfiAiHiEnhBNjibjMsQE5YfLUz0NqRZR-kKgE27WhabhBWxW7VEXhzW8SWDXtb0gsKxoDVSG8kIv1aEjn8BdutSe9xaA00160E2NwV0KgPUcUwF";

            var actual = Task.Run<RecaptchaValidationResult>(async () => await recaptcha.ValidateAsync(recaptchaResponse)).Result;

            Assert.IsFalse(actual.Success);
        }
    }
}
