﻿using System.Windows;
using PictOgr.SplashScreen.ViewModels;

namespace PictOgr.SplashScreen.Views
{
    /// <summary>
    /// Interaction logic for SplashScreenView.xaml
    /// </summary>
    public partial class SplashScreenView : Window
    {
        public SplashScreenView()
        {
            InitializeComponent();
            this.DataContext = new SplashScreenViewModel();
        }
    }
}
