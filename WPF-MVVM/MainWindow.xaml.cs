﻿using System.Windows;

namespace WPF_MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ArgbColorModel();

            MessageBox.Show("Fixed gub with login");
        }
    }
}
