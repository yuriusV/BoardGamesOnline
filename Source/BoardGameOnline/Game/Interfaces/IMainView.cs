using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Game.Interfaces
{
    public interface IMainView
    {
        Action WindowLoaded { get; set; }
        Action Minimized { get; set; }
        Action Unminimized { get; set; }
        Action ClosingWindow { get; set; }

        Func<object, bool> OnChatMessage { get; set; }

        void ShowChat( );
        void HideChat( );
        void ClearChat( );
        void AddMessageToChat( object message );
        void SetChatTitle( object title);

        void RegisterMainMenuItem( string title, Action clicked, Dictionary<string, Action> subitems = null);
        void SetDefaultView( UserControl control );
        void SetupView( UserControl control);
        void ShowMessage( string message );
        void ShowError( string error );
        void RegisterMainMenuItem( string v, Action p );
    }
}
