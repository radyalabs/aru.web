using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Helpers
{
    public class ErrorHelper
    {
        public static IEnumerable<string> Error(ModelStateDictionary modelStateDictionary)
        {
            List<string> errors = new List<string>();

            foreach(var item in modelStateDictionary)
            {
                if (item.Value != null && item.Value.Errors != null)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        if (string.IsNullOrEmpty(error.ErrorMessage))
                            errors.Add(error.Exception.Message);
                        else
                            errors.Add(error.ErrorMessage);
                    }
                }
            }

            return errors;
        }
    }
}
