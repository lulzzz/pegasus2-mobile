﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = new MainPageViewModel();                           
        }

    }
}
