﻿using Ecliptic.Data;
using Ecliptic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Ecliptic.Views.UserInteraction
{
    public partial class Authorization : ContentPage
    {
        static public class UserControls
        {
            static public Label labelMessage { get; set; }
            static public Label Login { get; set; }
            static public Label Pass { get; set; }
            static public Button LoginOutBtn { get; set; }
            static public List<Editor> Editors { get; set; }
            static public List<Switch> Switches { get; set; }
        }

        public void GetUserPage()
        {
            Title = "Профиль " + User.CurrentUser.Name;

            #region CreateControls
            this.ToolbarItems.Clear();
            UserControls.labelMessage = new Label
            {
                Text = User.CurrentUser.Name,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Center
            };
            UserControls.Login        = new Label
            {
                Text = User.CurrentUser.Login,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Center
            };
            UserControls.Pass         = new Label
            {
                Text = User.CurrentUser.Password,
                Style = Device.Styles.TitleStyle,
                HorizontalOptions = LayoutOptions.Center
            };
            UserControls.LoginOutBtn  = new Button
            {
                Text = "Login Out",
            };
            UserControls.Editors      = new List<Editor>();
            UserControls.Switches     = new List<Switch>();
            UserControls.LoginOutBtn.Clicked += GoLoginPage;
            #endregion

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            stackLayout.Children.Add(UserControls.labelMessage);
            stackLayout.Children.Add(UserControls.Login);
            stackLayout.Children.Add(UserControls.Pass);

            foreach (var i in User.CurrentUser.Notes)
            {
                Grid grid = new Grid
                {
                    RowDefinitions = {
                                     new RowDefinition { Height = new GridLength(30) },
                                     new RowDefinition { Height = new GridLength(20) },
                                     new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                        },
                    ColumnDefinitions = {
                                     new ColumnDefinition { Width = new GridLength(160) },
                                     new ColumnDefinition { Width = new GridLength(50) },
                                     new ColumnDefinition { Width = new GridLength(30) },
                                     new ColumnDefinition { Width = new GridLength(30) }
                        }

                };
                grid.ColumnSpacing = 10;
                grid.RowSpacing    = 10;

                Switch switche = new Switch
                {
                    IsToggled = i.isPublic,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    AutomationId = i.Id.ToString(),
                };
                Label  noteLab = new Label
                {
                    Text = i.Room.ToString() + " Аудитория ",
                    FontSize = 14,
                    Style = Device.Styles.TitleStyle,
                };
                Label  noteBui = new Label
                {
                    Text = "Здание "  + i.Building.ToString(),
                    FontSize = 14,
                    Style = Device.Styles.TitleStyle,
                };
                Editor noteEnt = new Editor
                {
                    AutoSize = EditorAutoSizeOption.TextChanges,
                    Text = i.Text.ToString() ?? "wot",
                    FontSize = 12,
                    Style = Device.Styles.BodyStyle,
                    AutomationId = i.Id.ToString(),
                };
                ImageButton SaveBtn = new ImageButton
                {
                    Source = "save.png",
                    AutomationId = i.Id.ToString(),
                }; 
                ImageButton DeleBtn = new ImageButton
                {
                    Source = "delete.png",
                    AutomationId = i.Id.ToString(),
                }; 

                UserControls.Editors.Add(noteEnt);
                UserControls.Switches.Add(switche);
                SaveBtn.Clicked += OnButtonSaveClicked;
                DeleBtn.Clicked += OnButtonDeleteClicked;

                grid.Children.Add(noteLab, 0, 0);
                grid.Children.Add(switche, 1, 0);
                grid.Children.Add(SaveBtn, 2, 0);
                grid.Children.Add(DeleBtn, 3, 0);
                grid.Children.Add(noteBui, 0, 1);
                grid.Children.Add(noteEnt, 0, 2);

                Grid.SetColumnSpan(noteEnt, 4);   // растягиваем на 4 столбца

                Frame frame = new Frame()
                {
                    BorderColor = Color.ForestGreen,
                    AutomationId = i.Id.ToString(),
                };

                frame.Content = grid;

                stackLayout.Children.Add(frame);
            }

            stackLayout.Children.Add(UserControls.LoginOutBtn);

            ScrollView scrollView = new ScrollView();
            scrollView.Content = stackLayout;

            this.Content = scrollView;

            #region ToolBarItems
            ToolbarItem NewNote = new ToolbarItem
            {
                Text = "Новая заметка",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0,
            };
            ToolbarItem FavRoom = new ToolbarItem
            {
                Text = "Избранное",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1,
            };
            ToolbarItem LogOut  = new ToolbarItem
            {
                Text = "Выйти",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1,
            };
            NewNote.Clicked += OnNewNoteClicked;
            FavRoom.Clicked += OnFavRoomsClicked;
            LogOut.Clicked += GoLoginPage;
            this.ToolbarItems.Add(NewNote);
            this.ToolbarItems.Add(FavRoom);
            this.ToolbarItems.Add(LogOut);
            #endregion
        }
        void GoLoginPage(object sender, EventArgs args)
        {
            this.ToolbarItems.Clear();

            if (User.CurrentUser != null)
            {
                User.LoginOut();
            }

          //  foreach (var i in RoomData.Roooms)
          //  {
          //      if (i.Notes.Count > 0)
          //      {
          //          i.Notes = new List<Note>();
          //      }
          //  }

            GetLoginPage();
        }

        void OnButtonSaveClicked(object sender, EventArgs args)
        {
            ImageButton btn = (ImageButton)sender;

            // сохранить в пользователе 
            Note temp = null;

            int i = 0;
            for (; i < User.CurrentUser.Notes.Count; i++)
            {
                if (User.CurrentUser.Notes[i].Id == Int32.Parse(btn.AutomationId))
                {
                    temp = User.CurrentUser.Notes[i];
                    break;
                }
            }

            foreach (var editor in UserControls.Editors)
            {
                if (editor.AutomationId == btn.AutomationId)
                {
                    temp.Text = editor.Text;
                    break;
                }
            }
            foreach (var Cswitch in UserControls.Switches)
            {
                if (Cswitch.AutomationId == btn.AutomationId)
                {
                    temp.isPublic = Cswitch.IsToggled;
                    break;
                }
            }

            User.CurrentUser.Notes[i] = temp;

            // отправить на сервер
            SendToDatabase(btn.AutomationId);
        }

        void SendToDatabase(string room)
        {

        }

        void OnButtonDeleteClicked(object sender, EventArgs args)
        {
            ImageButton btn = (ImageButton)sender;

            User.DeleteNote(Int32.Parse(btn.AutomationId));

            GetUserPage();
        }

        // Toolbar
        async void OnNewNoteClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new NewNotePage("",""));
        }
        async void OnFavRoomsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new FavRoomsPage());
        }
    }
}