using System;

namespace General.Validation
{
    public interface IValidatorProvider
    {
        IValidator GetValidator(Type type);
    }
}
