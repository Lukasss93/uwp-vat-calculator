﻿using AuraRT.Display;
using AuraRT.Localization;
using AuraRT.Storage;
using AuraRT.Utilities;
using Lukasss93.Controls;
using NotificationsExtensions.TileContent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace VAT_Discount_Calculator.Pages
{
    public sealed partial class Home : Page
    {
        private double vat = 22;
        private double discount = 50;

        public Home()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Utility.SetMargin(TITLE, Utility.MarginEdge.Top, StatusBar.GetForCurrentView().OccludedRect.Height);
            StatusBar.GetForCurrentView().BackgroundOpacity = 0.2;
            
            flip.SelectionChanged += Flip_SelectionChanged;
            tab_vat.Tapped += (s, e) => { flip.SetValue(FlipView.SelectedIndexProperty, 0); };
            tab_discount.Tapped += (s, e) => { flip.SetValue(FlipView.SelectedIndexProperty, 1); };

            cb_tile.Click += Cb_tile_Click;

            //vat
            vat = Convert.ToDouble((int)AppSettings.Get("vat_percentage"));
            vat_slider.Value = vat;
            vat_percentage.Text = vat + "%";
            vat_slider.ValueChanged += Vat_slider_ValueChanged;
            vat_enter_number.KeyUp += (s, e) => { CalculateVAT(); };

            //discount
            discount = Convert.ToDouble((int)AppSettings.Get("discount_percentage"));
            discount_slider.Value = discount;
            discount_percentage.Text = discount + "%";
            discount_slider.ValueChanged += Discount_slider_ValueChanged;
            discount_enter_number.KeyUp += (s, e) => { CalculateDiscount(); };

            //localizzazione
            titletext.Text = Translate.Get("AppName").ToUpper();

            tab_vat.TabHeader = Translate.Get("vat");
            vat_percentage_title.Text = Translate.Get("percentage_vat");
            vat_enter_number.Header = Translate.Get("enter_number");
            vat_result_tile.Text = Translate.Get("result");

            tab_discount.TabHeader = Translate.Get("discount");
            discount_percentage_title.Text = Translate.Get("percentage_discount");
            discount_enter_number.Header = Translate.Get("enter_number");
            discount_result_title.Text = Translate.Get("result");

            GoogleAnalytics.EasyTracker.GetTracker().SendView("Home.xaml.cs");
        }

        private void Cb_tile_Click(object sender, RoutedEventArgs e)
        {
            if((bool)AppSettings.Get("transparent_tile"))
            {
                AppSettings.Set("transparent_tile", false);
            }
            else
            {
                AppSettings.Set("transparent_tile", true);
            }

            CheckTile();

            GoogleAnalytics.EasyTracker.GetTracker().SendEvent("Home.xaml.cs", "Cb_tile_Click", null, 0);
        }

        private void CheckTile()
        {
            string folderName = "colored";
            cb_tile.Label = String.Format(Translate.Get("transparent_tile"), Translate.Get("off"));

            if((bool)AppSettings.Get("transparent_tile"))
            {
                folderName = "transparent";
                cb_tile.Label = String.Format(Translate.Get("transparent_tile"), Translate.Get("on"));
            }

            var small = TileContentFactory.CreateTileSquare71x71Image();
            small.Image.Src = "ms-appx:///Assets/tile/" + folderName + "/170x170.png";

            var normal = TileContentFactory.CreateTileSquare150x150Image();
            normal.Image.Src = "ms-appx:///Assets/tile/" + folderName + "/360x360.png";

            normal.Square71x71Content = small;

            TileUpdateManager.CreateTileUpdaterForApplication().Update(normal.CreateNotification());
        }

        #region DISCOUNT
        private void CalculateDiscount()
        {
            try
            {
                double value_entered = Convert.ToDouble(discount_enter_number.Text);

                double discount_toremove = (value_entered * discount) / 100;
                double discount_calculated = value_entered - discount_toremove;

                discount_result.Text = discount_calculated.ToString();
            }
            catch
            {
                discount_result.Text = "0";
            }
        }

        private void Discount_slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            int newvalue = (int)e.NewValue;
            AppSettings.Set("discount_percentage", newvalue);
            discount_percentage.Text = newvalue + "%";
            discount = newvalue;
            CalculateDiscount();
        }
        #endregion

        #region VAT
        private void CalculateVAT()
        {
            try
            {
                double value_entered = Convert.ToDouble(vat_enter_number.Text);

                double vat_toadd = (value_entered * vat) / 100;
                double vat_calculated = value_entered + vat_toadd;

                vat_result.Text = vat_calculated.ToString();
            }
            catch
            {
                vat_result.Text = "0";
            }
        }

        private void Vat_slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            int newvalue = (int)e.NewValue;
            AppSettings.Set("vat_percentage", newvalue);
            vat_percentage.Text = newvalue + "%";
            vat = newvalue;
            CalculateVAT();
        }
        #endregion

        private void Flip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tab_vat.TabIsSelected = false;
            tab_discount.TabIsSelected = false;

            switch(flip.SelectedIndex)
            {
                case 0: tab_vat.TabIsSelected = true; break;
                case 1: tab_discount.TabIsSelected = true; break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CheckTile();
        }
    }
}
