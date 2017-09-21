using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimaryInterface1._0.Model
{
    class Model:NotificatinObject
    {
        private string _deviceName;
        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                this.RaisePropertyChanged("DeviceName");
            }
        }

        private int _interfaceCount;
        public int InterfaceCount
        {
            get { return _interfaceCount; }
            set
            {
                _interfaceCount = value;
                this.RaisePropertyChanged("InterfaceCount");
            }
        }

        public Model(string deviceName,int interfaceCount)
        {
            DeviceName = deviceName;
            InterfaceCount = interfaceCount;
        }
    }
}
