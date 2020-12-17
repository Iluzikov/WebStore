using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public record AjaxTestDataViewModel(int? id, string message, DateTime serverTime);
}
