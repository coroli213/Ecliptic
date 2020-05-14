﻿using Xamarin.Forms;
using System;
using System.Net.Http;
using Ecliptic.Models;
using Plugin.Connectivity;
using Newtonsoft.Json;
using Ecliptic.WebInteractions;
using Newtonsoft.Json.Linq;
using Ecliptic.Repository;
using System.Collections.Generic;
using System.Text;
using Plugin.Connectivity.Abstractions;
using System.Linq;
using System.Net;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Essentials;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        public class LoginControls
        {
            public Label TitleLab { get; set; }
            public Entry LoginBox { get; set; }
            public Entry PasswBox { get; set; }
            public Button LoginBtn { get; set; }
            public Button RegisBtn { get; set; }

            public LoginControls()
            {
                TitleLab = new Label
                {
                    Text = "Вход",
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.Center,
                }; 

                LoginBox = new Entry
                {
                    Text = "",
                    Placeholder = "Имя",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };
                PasswBox = new Entry
                {
                    Text = "",
                    Placeholder = "Пароль",
                    Keyboard = Keyboard.Default,
                    TextColor = Color.Black,
                    PlaceholderColor = Color.Black,
                    IsPassword = true,
                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
                    Style = Device.Styles.BodyStyle,
                    HorizontalOptions = LayoutOptions.Fill
                };

                LoginBtn = new Button
                {
                    Text = "Войти",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };
                RegisBtn = new Button
                {
                    Text = "Зарегестрироваться",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };
            }
        }

        private LoginControls LoginPage;

        public void GetLoginPage()
        {
            Title = "Войти";

            LoginPage = new LoginControls();

            LoginPage.LoginBtn.Clicked += LoginIn;
            LoginPage.RegisBtn.Clicked += ToRegistrationPage;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;      
 
            stackLayout.Children.Add(new BoxView { VerticalOptions = LayoutOptions.CenterAndExpand });
            stackLayout.Children.Add(LoginPage.TitleLab);
            stackLayout.Children.Add(LoginPage.LoginBox);
            stackLayout.Children.Add(LoginPage.PasswBox);
            stackLayout.Children.Add(LoginPage.LoginBtn);
            stackLayout.Children.Add(LoginPage.RegisBtn);
            stackLayout.Children.Add(new BoxView { VerticalOptions = LayoutOptions.CenterAndExpand });

            this.Content = stackLayout;
        }

        public async void LoginIn(object sender, EventArgs e)
        {
            if (LoginPage.LoginBox.Text == "" || LoginPage.PasswBox.Text == "")
            {
                DependencyService.Get<IToast>().Show("Введены не все поля");
                return;
            }

            // тестовый пользователь
            if (WebData.istest)
            {
                User.LoadUser(LoginPage.LoginBox.Text, LoginPage.PasswBox.Text);
                GetUserPage();
                return;
            }

            if (CrossConnectivity.Current.IsConnected == false)
            {
                DependencyService.Get<IToast>().Show("Устройство не подключено к сети");
                return;
            }

            bool isRemoteReachable = await CrossConnectivity.Current.IsReachable("ecliptic.geo.com");
            if (!isRemoteReachable)
            { 
                await DisplayAlert("Сервер не доступен", "Повторите попытку позже", "OK");
                return;
            }

            UserService userService = new UserService();
            User user = await userService.Get(LoginPage.LoginBox.Text, LoginPage.PasswBox.Text);
                 
            // если сервер вернул данные пользователя - загрузить в пользователя
            if (user != null)
            {
                NoteService    noteService    = new NoteService();
                FavRoomService favRoomService = new FavRoomService();

                DbService.SaveUser(user); // сохранили пользователя
                DbService.AddNote        (await noteService   .Get(user.UserId)); // сохранили его заметки
                DbService.AddFavoriteRoom(await favRoomService.Get(user.UserId)); // и избраные помещения

                DbService.LoadUser();
                GetUserPage();

                return;
            }
            else
            {
                await DisplayAlert("Ошибка", "Данные с сервера не получены", "OK");
                return;
            }
        }

        private void ToRegistrationPage(object sender, EventArgs e)
        {
            GetRegistrationPage();
        }
    }
}
