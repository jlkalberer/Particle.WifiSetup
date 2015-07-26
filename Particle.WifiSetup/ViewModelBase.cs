using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particle.WifiSetup
{
    using System.ComponentModel;

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
     
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
