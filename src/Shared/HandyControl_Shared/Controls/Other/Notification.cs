﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using HandyControl.Data;
using HandyControl.Tools;

namespace HandyControl.Controls;

public sealed class Notification : System.Windows.Window
{
    private int WaitTime;

    /// <summary>
    ///     计数
    /// </summary>
    private int _tickCount;

    /// <summary>
    ///     关闭计时器
    /// </summary>
    private DispatcherTimer _timerClose;

    private ShowAnimation ShowAnimation { get; set; }

    private bool _shouldBeClosed;

    public Notification()
    {
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
    }

    public static void Init(object content, ShowAnimation showAnimation, int waitTime, int offSet = 0)
    {
        var notification = new Notification
        {
            Content = content,
            Opacity = 0,
            ShowAnimation = showAnimation
        };

        notification.Show();

        var desktopWorkingArea = SystemParameters.WorkArea;
        var leftMax = desktopWorkingArea.Width - notification.ActualWidth - offSet;
        var topMax = desktopWorkingArea.Height - notification.ActualHeight - offSet;

        switch (showAnimation)
        {
            case ShowAnimation.None:
                notification.Opacity = 1;
                notification.Left = leftMax;
                notification.Top = topMax;
                break;
            case ShowAnimation.HorizontalMove:
                notification.Opacity = 1;
                notification.Left = desktopWorkingArea.Width;
                notification.Top = topMax;
                notification.BeginAnimation(LeftProperty, AnimationHelper.CreateAnimation(leftMax));
                break;
            case ShowAnimation.VerticalMove:
                notification.Opacity = 1;
                notification.Left = leftMax;
                notification.Top = desktopWorkingArea.Height;
                notification.BeginAnimation(TopProperty, AnimationHelper.CreateAnimation(topMax));
                break;
            case ShowAnimation.Fade:
                notification.Left = leftMax;
                notification.Top = topMax;
                notification.BeginAnimation(OpacityProperty, AnimationHelper.CreateAnimation(1));
                break;
            default:
                notification.Opacity = 1;
                notification.Left = leftMax;
                notification.Top = topMax;
                break;
        }

        if (waitTime > 0)
        {
            notification.WaitTime = waitTime;
            notification.StartTimer();
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        if (_shouldBeClosed)
        {
            return;
        }
        var desktopWorkingArea = SystemParameters.WorkArea;

        switch (ShowAnimation)
        {
            case ShowAnimation.None:
                break;
            case ShowAnimation.HorizontalMove:
                {
                    var animation = AnimationHelper.CreateAnimation(desktopWorkingArea.Width);
                    animation.Completed += Animation_Completed;
                    BeginAnimation(LeftProperty, animation);
                    e.Cancel = true;
                    _shouldBeClosed = true;
                }
                break;
            case ShowAnimation.VerticalMove:
                {
                    var animation = AnimationHelper.CreateAnimation(desktopWorkingArea.Height);
                    animation.Completed += Animation_Completed;
                    BeginAnimation(TopProperty, animation);
                    e.Cancel = true;
                    _shouldBeClosed = true;
                }
                break;
            case ShowAnimation.Fade:
                {
                    var animation = AnimationHelper.CreateAnimation(0);
                    animation.Completed += Animation_Completed;
                    BeginAnimation(OpacityProperty, animation);
                    e.Cancel = true;
                    _shouldBeClosed = true;
                }
                break;
        }
    }

    private void Animation_Completed(object sender, EventArgs e) => Close();

    /// <summary>
    ///     开始计时器
    /// </summary>
    private void StartTimer()
    {
        _timerClose = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timerClose.Tick += delegate
        {
            if (IsMouseOver)
            {
                _tickCount = 0;
                return;
            }

            _tickCount++;
            if (_tickCount >= WaitTime) Close();
        };
        _timerClose.Start();
    }
}
