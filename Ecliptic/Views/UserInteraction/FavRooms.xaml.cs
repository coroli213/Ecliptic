﻿using Ecliptic.Data;
using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class FavRoomsPage : ContentPage
    {
        public FavRoomsPage()
        {
            InitializeComponent();

            Title = "Избоанные аудитории";

            RoomView.ItemsSource = User.CurrentUser.Favorites;
        }

        private async void RoomView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string roomName = (e.Item as Room).Name;

            await Shell.Current.GoToAsync($"roomdetails?name={roomName}");
        }

        protected override void OnAppearing()
        {
            RoomView.ItemsSource = User.CurrentUser.Favorites;

            base.OnAppearing();
        }
    }
}