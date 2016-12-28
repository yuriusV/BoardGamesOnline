using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Presenters
{
    public class NetworkInformationPresenter
    {
        public NetworkInformationPresenter( ) {

        }

        public void Register( ) {
            string info = "";
            foreach(NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    info += ni.Name + "\n";

                    foreach(UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if(ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            info += ip.Address.ToString() + "\n";
                        }
                    }
                }
            }

            MessageBox.Show(info, "Ваши данные подключения", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
