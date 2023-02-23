using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControlDemo.UserControl;

namespace HandyControlDemo.ViewModel;

public class NotificationDemoViewModel : ViewModelBase
{
    public RelayCommand OpenCmd => new(() => Notification.Show(new AppNotification(), ShowAnimation, WaitTime));

    private ShowAnimation _showAnimation;

    public ShowAnimation ShowAnimation
    {
        get => _showAnimation;
#if NET40
        set => Set(nameof(ShowAnimation) ,ref _showAnimation, value);
#else
        set => Set(ref _showAnimation, value);
#endif
    }

    private int _waitTime = 5;

    public int WaitTime
    {
        get => _waitTime;
#if NET40
        set => Set(nameof(WaitTime) ,ref _waitTime, value);
#else
        set => Set(ref _waitTime, value);
#endif
    }
}
