using Trisatech.MWorkforce.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Interfaces
{
    public interface IRefService
    {
        IEnumerable<ReasonViewModel> GetReasons();
    }
}
