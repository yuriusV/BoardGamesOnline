using Game.Interfaces;
using Game.Settings;
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

namespace Game.Views.Default
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl, ISettingsView
    {
        private Dictionary<string, Dictionary<string, Tuple<string, string>>> _data;

        public Settings( )
        {
            InitializeComponent();
        }



        #region [ISettingsView implemenation]
        public Action Cancel { get; set; }

        public Action<Dictionary<string, Dictionary<string, Tuple<string, string>>>> Save { get; set; }

        public Action<string, string, string> Updated { get; set; }

        public void DisplaySettings( Dictionary<string, Dictionary<string, Tuple<string, string>>> data )
        {
            _data = data;

            foreach(var module in data) {
                var tab = new TabItem();
                tab.Header = module.Key;
                SetupTab(tab, module.Value);
                mainTab.Items.Add(tab);
            }

            
        }

        private void SetupTab( TabItem tab, Dictionary<string, Tuple<string, string>> data ) {
            var stack = new StackPanel();

            var boolSettings = data.Where(x => x.Value.Item2 == "bool");
            if(boolSettings.Any()) {
                var header = new TextBlock();
                header.Text = "Основные";

                foreach(var seti in boolSettings) {
                    var check = new CheckBox();
                    check.Content = seti.Key;
                    check.IsChecked = (seti.Value.Item1 == "1") ? true : false;

                    check.Checked += ( s, e ) => {
                        _data[tab.Header.ToString()][seti.Key] = new Tuple<string, string>("1", "bool");
                    };

                    check.Unchecked += ( s, e ) => {
                        _data[tab.Header.ToString()][seti.Key] = new Tuple<string, string>("0", "bool");
                    };

                }

                stack.Children.Add(new Separator());
            }

            foreach(var item in data) {
                var keyHeader = new TextBlock() { Text = item.Key };

                if(item.Value.Item2 == "string")
                {
                    var textBox = new TextBox();
                    textBox.Text = item.Value.Item1;

                    textBox.TextChanged += ( s, e ) => {
                        _data[tab.Header.ToString()][item.Key] = new Tuple<string, string>(textBox.Text, "string");
                        Updated?.Invoke(tab.Header.ToString(), item.Key, item.Value.Item1);
                    };
                }
                else if(item.Value.Item2 == "int")
                {
                    var textBox = new TextBox();
                    textBox.Text = item.Value.Item1;

                    textBox.TextChanged += ( s, e ) => {
                        try { int a = int.Parse(textBox.Text); }
                        catch { e.Handled = true; return; }
                        _data[tab.Header.ToString()][item.Key] = new Tuple<string, string>(textBox.Text, "int");
                        Updated?.Invoke(tab.Header.ToString(), item.Key, item.Value.Item1);
                    };
                }
                else if(item.Value.Item2 == "double")
                {
                    var textBox = new TextBox();
                    textBox.Text = item.Value.Item1;

                    textBox.TextChanged += ( s, e ) => {
                        try { var a = double.Parse(textBox.Text); }
                        catch { e.Handled = true; return; }
                        _data[tab.Header.ToString()][item.Key] = new Tuple<string, string>(textBox.Text, "float");
                        Updated?.Invoke(tab.Header.ToString(), item.Key, item.Value.Item1);
                    };
                }
                else if(item.Value.Item2 == "list_string") {
                    var combo = new ComboBox() { };
                    var items = SettingsManager.ConvertFromString(item.Value.Item1, "list_string")as List<string>;
                    foreach(var arrItem in items) {
                        combo.Items.Add(arrItem);
                    }

                    combo.SelectionChanged += ( s, e ) => {
                        var selected = combo.SelectedItem.ToString();
                        var old = _data[tab.Header.ToString()][item.Key].Item1;
                        old = selected + "," + old.Replace(selected, "").Replace(",,", ",");
                        _data[tab.Header.ToString()][item.Key] = new Tuple<string, string>(old, "list_string");
                    };
                }
                
                stack.Children.Add(new Separator());
            }
        }

        #endregion
    }
}
