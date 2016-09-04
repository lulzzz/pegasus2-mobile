using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegasusNAEMobile.ViewModels
{
    public class BasicViewModel<T> : BaseViewModel
    {
        private T data;
        public T Data
        {
            get { return data; }
            set { SetProperty(ref data, value); }
        }

        protected BasicViewModel()
        {
        }
    }
}
