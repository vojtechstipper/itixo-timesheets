using DotVVM.Framework.ViewModel;
using DotVVM.Framework.ViewModel.Validation;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Admin.Client.ViewModels.Base;

public class ViewModelBase : DotvvmViewModelBase
{
    public void ProcessValidations<T>(ApiResult<T> apiResult)
    {
        foreach (KeyValuePair<string, string[]> validation in apiResult.Validations)
        {
            foreach (string messageResourceName in validation.Value)
            {
                string message = Validations.ResourceManager.GetString(messageResourceName);

                this.AddModelError($"{message} {validation.Key}");
            }
        }
    }
}
