﻿using FolderCreator.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Xml;

namespace FolderCreator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel MainWindowViewModel { get; set; } = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainWindowViewModel;
            //this.dgvWorkItems.ItemsSource = MainWindowViewModel.WiDetails;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MainWindowViewModel.FolderPath = @"C:\Users\Sandeep.shenoy\OneDrive - Noxum GmbH\Work Details";
        }
    }
}