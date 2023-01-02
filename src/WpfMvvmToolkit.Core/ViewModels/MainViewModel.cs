using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfMvvmToolkit.Core.Services;
using Timer = System.Timers.Timer;

namespace WpfMvvmToolkit.Core.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private ILogService Logger { get; }

        public MainViewModel(ILogService logService)
        {
            Logger = logService;

            Title = "MVVM Toolkit Main Window";
            Logger.Debug($"Title : {Title}");
            
            TotalCount = 10;
            IsBusy = false;
        }

        private int _totalCount;
        public int TotalCount
        {
            get { return _totalCount; }
            set { SetProperty(ref _totalCount, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public string Button1Content { get; set; } = "Countdown using sync";
        public string Button2Content { get; set; } = "Countdown using async";

        private RelayCommand _doCountDownCommand;
        public IRelayCommand DoCountDownCommand => 
                _doCountDownCommand ?? (_doCountDownCommand = new RelayCommand(DoCountDown));

        DispatcherTimer _dispatcherTimer;
        Timer _timer;

        private void DoCountDown()
        {
            IsBusy = true;

            ////////////////////////////////////////////////////
            // Worker Thread
            ////////////////////////////////////////////////////
            //Button1Content = "Countdown using loop on Worker thread"
            //Task.Run(async () =>
            //{
            //    while (TotalCount > 0)
            //    {
            //        await Task.Delay(TimeSpan.FromSeconds(1));
            //        TotalCount--;
            //    }
            //    IsBusy = false;
            //});

            ////////////////////////////////////////////////////
            // DispatcherTimer
            // - 반드시 UI 쓰레드를 사용해야 한다.
            // - Worker 쓰레드 사용시 timer 동작 하지 않음.
            ////////////////////////////////////////////////////
            //Button1Content = "Countdown using DispatcherTimer on UI thread"
            //_dispatcherTimer = new DispatcherTimer();
            //_dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            //_dispatcherTimer.Tick += (s, e) =>
            //{
            //    if (TotalCount > 0)
            //    {
            //        TotalCount--;
            //    }
            //    else
            //    {
            //        _dispatcherTimer.Stop();
            //        IsBusy = false;
            //    }
            //};
            //_dispatcherTimer.Start();

            ////////////////////////////////////////////////////
            // System.Timers.Timer
            // - UI, Worker 쓰레드 상관없이 timer 동작함.
            ////////////////////////////////////////////////////
            Button1Content = "Countdown using System Timer on Worker thread";
            Task.Run(() =>
            {
                _timer = new Timer(1000);
                _timer.Elapsed += (s, e) =>
                {
                    if (TotalCount > 0)
                    {
                        TotalCount--;
                    }
                    else
                    {
                        _timer.Stop();
                        _timer.Close();
                        IsBusy = false;
                    }
                };
                _timer.Start();
            });
        }

        private AsyncRelayCommand _doCountDownAsyncCommand;
        public IRelayCommand DoCountDownAsyncCommand =>
                _doCountDownAsyncCommand ?? (_doCountDownAsyncCommand = new AsyncRelayCommand(DoCountDownAsync));

        private async Task DoCountDownAsync()
        {
            IsBusy = true;

            //Button2Content = "Async Countdown using loop on UI thread";
            //while (TotalCount > 0)
            //{
            //    await Task.Delay(TimeSpan.FromSeconds(1));
            //    TotalCount--;
            //}
            //IsBusy = false;

            //Button2Content = "Async Countdown using loop on async/await worker thread";
            //await Task.Run(() =>
            //{
            //    while (TotalCount > 0)
            //    {
            //        Thread.Sleep(TimeSpan.FromSeconds(1));
            //        TotalCount--;
            //    }

            //    IsBusy = false;
            //});

            //Button2Content = "Async Countdown using loop on worker thread";
            //_ = Task.Run(() =>
            //{
            //    while (TotalCount > 0)
            //    {
            //        Thread.Sleep(TimeSpan.FromSeconds(1));
            //        TotalCount--;
            //    }

            //    IsBusy = false;
            //});

            Button2Content = "Async Countdown using loop on async worker thread";
            _ = Task.Run(async () =>
            {
                while (TotalCount > 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    TotalCount--;
                }

                IsBusy = false;
            });

            //await Task.Run(async () =>
            //{
            //    while (TotalCount > 0)
            //    {
            //        await Task.Delay(TimeSpan.FromSeconds(1));
            //        TotalCount--;
            //    }

            //    IsBusy = false;
            //});
        }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        [NotifyCanExecuteChangedFor(nameof(SayHello))]
        private string name;

        private string lastName = "Han";

        private string FullName
        {
            get => $"{lastName} {name}";
        }

        [RelayCommand]
        private void SayHello()
        {
            Logger.Debug($"Hello, {name}!");
        }

        [RelayCommand]
        private void SayFormalHello()
        {
            Logger.Debug($"Hello, Mr. {FullName}!");
        }
    }

}
