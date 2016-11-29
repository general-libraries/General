using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using General.Helpers;

namespace General.Web.Api.ModelBinders
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class ObfuscatedAttribute : ModelBinderAttribute
    {
        public ObfuscatedAttribute()
            : base(typeof(ObfuscatedModelBinder))
        { }
    }

    public class ObfuscatedModelBinder : IModelBinder
    {
        public static bool Enabled
        {
            get { return Manager.Instance.GlobalSetting.IdObfuscation; }
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (val == null) return false;

            var decrypted =
                Enabled
                    ? HashHelper.Decode(val.RawValue as string, bindingContext.ModelType)
                    : val.RawValue;

            if (decrypted != null)
            {
                try
                {
                    bindingContext.Model =
                        decrypted.GetType() == bindingContext.ModelType
                            ? decrypted
                            : Convert.ChangeType(decrypted, bindingContext.ModelType);
                    return true;
                }
                catch
                {
                    // ignored
                }
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "cannot-convert-value");
            return false;
        }
    }
}
