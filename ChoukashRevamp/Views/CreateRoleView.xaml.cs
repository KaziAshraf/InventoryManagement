﻿using System;
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

namespace ChoukashRevamp.Views
{
    /// <summary>
    /// Interaction logic for CreateRoleView.xaml
    /// </summary>
    public partial class CreateRoleView : UserControl
    {
        public CreateRoleView()
        {
            InitializeComponent();
            this.Loaded += CreateRoleView_Loaded;
        }

        void CreateRoleView_Loaded(object sender, RoutedEventArgs e)
        {
            this.RoleName.Focus();
        }
    }
}
