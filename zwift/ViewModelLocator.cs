using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zwift
{
    public class ViewModelLocator // : IViewModelLocator
    {
        public WorkoutsViewModel WorkoutsViewModel { get; }

        public ViewModelLocator()
        {
            // todo: init properly and instantiate design vs production view models
            WorkoutsViewModel = new WorkoutsViewModel();

        }
    }
}
