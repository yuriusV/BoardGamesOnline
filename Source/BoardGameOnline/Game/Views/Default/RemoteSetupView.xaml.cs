using Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Game.Model.User;

namespace Game.Views.Default
{
    /// <summary>
    /// Interaction logic for RemoteSetupView.xaml
    /// </summary>
    public partial class RemoteSetupView : UserControl, IRemoteSetupView
    {

        
        public RemoteSetupView( )
        {
            InitializeComponent();
        }


        #region [MVVM]


        public string ConnectIP
        {
            get { return (string)GetValue(ConnectIPProperty); }
            set { SetValue(ConnectIPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectIP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectIPProperty =
            DependencyProperty.Register("ConnectIP", typeof(string), typeof(RemoteSetupView), new PropertyMetadata(""));




        public string ConnectPort
        {
            get { return (string)GetValue(ConnectPortProperty); }
            set { SetValue(ConnectPortProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectPort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectPortProperty =
            DependencyProperty.Register("ConnectPort", typeof(string), typeof(RemoteSetupView), new PropertyMetadata(""));




        public List<User> ContactsList
        {
            get { return (List<User>)GetValue(ContactsListProperty); }
            set { SetValue(ContactsListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Contacts.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContactsListProperty =
            DependencyProperty.Register("ContactsList", typeof(List<User>), typeof(RemoteSetupView), new PropertyMetadata(new List<User>()) );



        #endregion


        #region [IRemoteSetup implementation]
        public Action Cancel { get; set; }
        public List<User> Contacts { get;  set; }
        public Action<string> StartGame { get; set; }

        public void Start( )
        {
            ContactsList = Contacts;
            DataContext = this;

            buttonStart.Click += ( s, e ) => {
                StartGame?.Invoke(ConnectIP + ":" + ConnectPort);
            };

            buttonCancel.Click += ( s, e ) => {
                Cancel?.Invoke();
            };
        }

        #endregion

        private void TextBlock_MouseDown( object sender, MouseButtonEventArgs e )
        {
            var text = sender as TextBlock;
            if(text == null)
                return;

            ConnectIP = ContactsList
                .FirstOrDefault(x => x.Name == text.Text)?.Address?.Split(':')[0] ?? ConnectIP;

            ConnectPort = ContactsList
                .FirstOrDefault(x => x.Name == text.Text)?.Address?.Split(':')[1] ?? ConnectPort;
        }
    }
}
