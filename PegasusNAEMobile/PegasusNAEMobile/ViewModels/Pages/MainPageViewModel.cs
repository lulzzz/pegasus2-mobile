using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegasusNAEMobile
{
    public class MainPageViewModel
    {
        private CountdownViewModel _countdown;
        public CountdownViewModel Countdown
        {
            get
            {
                if (_countdown == null)
                {
                    _countdown = new CountdownViewModel();
                }
                return _countdown;
            }
        }
    }
}
