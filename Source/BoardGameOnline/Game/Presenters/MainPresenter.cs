﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using System.Windows.Controls;
using Game.Views.Default;
using Game.Model.Game.Chess;
using Game.Settings;

namespace Game.Presenters
{
    public class MainPresenter
    {

        private static MainPresenter _instance;
        public static MainPresenter Get( ) {
            if(_instance == null)
                _instance = new MainPresenter();

            return _instance;
        }
        private bool _nowProcessing = false;

        private LinkedList<Func<object, bool>> _messageListeners;
        private IMainView _mainView = null;

        
        public MainPresenter( ) {
            _instance = this;
            _messageListeners = new LinkedList<Func<object, bool>>();
        }
        
        public void SetupView( IMainView view ) {
            _mainView = view;

            view.RegisterMainMenuItem("Новая игра", ( ) => { }, new Dictionary<string, Action>
            {
                ["Против ПК"] = () => StartGameAgainstPC(),
                ["Сам с собой"] = () => StartSingleGame(),
                ["Удаленно"] = () => StartRemoteGame(),
            });
            view.RegisterMainMenuItem("Настройки", ( ) => StartSettings(), null);
            view.RegisterMainMenuItem("Детали", ( ) => { }, new Dictionary<string, Action> {
                ["Статистика"] = StartStatistics,
                ["Игроки"] = StartGetGamers,
            });
            view.RegisterMainMenuItem("Мой IP", ( ) =>
            {
                var pres = new NetworkInformationPresenter();
                pres.Register();
            });


            view.HideChat();
            view.OnChatMessage = ChatMessageRecived;

            view.SetDefaultView(new MainScreen());


            view.ClosingWindow = ( ) => {
                SettingsManager.Save();
            };
        }

        private void StartGetGamers( ) {

        }

        private void StartStatistics( )
        {
            var statPres = new StatisticPresenter();
            statPres.Register();    
        }

        private void StartRemoteGame( )
        {
            var pres = new RemoteSetupPresenter();
            pres.Register();
        }

        private void StartSettings( ) {
            _mainView.SetupView(new WaitIView());
            var pres = new SettingsPresenter();
            pres.Register();
        }



        public void AddMessageListener( Func<object, bool> message ) {
            _messageListeners.AddLast(message);
        }

        private void StartSingleGame( )
        {
            // TODO : remote 'chess', set 'game'
            var sets = new ChessSettings();

            sets.IsWhite = true;
            sets.Type = GameType.Single;
            sets.SecondLimited = -1;

            var pres = new ChessBoardPresenter();
            pres.Register(sets);
            _nowProcessing = true;
        }

        private void StartGameAgainstPC( ) {
            var sets = new ChessSettings();

            sets.IsWhite = true;
            sets.Type = GameType.AgainstPC;
            sets.SecondLimited = -1;

            var pres = new ChessBoardPresenter();
            pres.Register(sets);
            _nowProcessing = true;
        }
        
        public void ReleaseControl( ) {
            _mainView.SetupView(null);
        }

        public void ShowQuestion( string question, Action<bool> answered ) {
            _mainView.ShowQuestion(question, (res) => {
                answered(res);
            });
        }

        public void ShowInfo( object info ) {
            _mainView.ShowMessage(info as string);
        }

        public void ShowError( object error ) {
            _mainView.ShowError(error as string);
        }

        private bool ChatMessageRecived(object message ) {
            bool norm = true;
            foreach(var listeners in _messageListeners) {
                if(!listeners?.Invoke(message) ?? true)
                    norm = false;
            }
            return norm;
        }

        public bool QueryForSetupView( UserControl control ) {
            _mainView.SetupView(control);
            return true;
        }

        public void AddChatMessage( object message ) => _mainView.AddMessageToChat(message);
        public void ClearChat( ) => _mainView.ClearChat();
        public void HideChat( ) => _mainView.HideChat();
        public void ShowChat( ) => _mainView.ShowChat();
    }
}
 