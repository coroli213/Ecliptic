﻿using Ecliptic.Data;
using Ecliptic.Models;
using Ecliptic.Repository;
using Ecliptic.Views.ClientNote;
using Ecliptic.Views.FavoriteRoomList;
using System;
using System.Collections.Generic; 
using Xamarin.Forms;
using System.Linq;
using Ecliptic.WebInteractions;

namespace Ecliptic.Views.ClientInteraction
{
    public partial class Authorization : ContentPage
    {
        public class ClientControls
        {
            public Label NameLab  { get; set; }
            public Label LoginLab { get; set; }

            public Label NoteCount { get; set; }

            public List<Editor> Editors  { get; set; }
            public List<Switch> Switches { get; set; }
            public List<Label>  Dates    { get; set; }

            public Button LoginOutBtn { get; set; }

            public ClientControls()
            {
                NameLab   = new Label
                {
                    Text = Client.CurrentClient.Name,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                LoginLab  = new Label
                {
                    Text = Client.CurrentClient.Login,
                    Style = Device.Styles.TitleStyle,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                };
                NoteCount = new Label
                {
                    Text = "У вас " + Client.CurrentClient.Notes.Count + " заметок и " + Client.CurrentClient.Favorites.Count + " избраных",
                    Style = Device.Styles.ListItemTextStyle,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                LoginOutBtn = new Button
                {
                    Text = "Login Out",
                    BackgroundColor = Color.FromHex("#BFD9B6"),
                    TextColor = Color.Black,
                    BorderColor = Color.Black,
                };

                Editors  = new List<Editor>();
                Switches = new List<Switch>();
                Dates    = new List<Label> ();
            }
        }

        public ClientControls ClientPage;

        public void GetClientPage()
        {
            Title = "Профиль " + Client.CurrentClient.Name;

            LoginPage       = null;
            RegisrationPage = null;

            ClientPage = new ClientControls();
            ClientPage.LoginOutBtn.Clicked += GoLoginPage;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Margin = 20;

            stackLayout.Children.Add(ClientPage.NameLab);
            stackLayout.Children.Add(ClientPage.LoginLab);
            stackLayout.Children.Add(ClientPage.NoteCount);

            // заметки пользователя
            foreach (var note in Client.CurrentClient.Notes)
            {
                stackLayout.Children.Add(PaintNote(note));
            }

            stackLayout.Children.Add(ClientPage.LoginOutBtn);

            this.Content = new ScrollView { Content = stackLayout };

            #region ToolBarItems

            this.ToolbarItems.Clear();

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
            LogOut.Clicked  += GoLoginPage;

            this.ToolbarItems.Add(NewNote);
            this.ToolbarItems.Add(FavRoom);
            this.ToolbarItems.Add(LogOut);

            #endregion
        }


        private Frame PaintNote(Note note)
        {
            Label roomname     = new Label
            {
                Text = note.RoomName.ToString(),
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };
            Label buildingname = new Label
            {
                Text = note.Building != "" ? "Здание " + note.Building.ToString() : "",
                FontSize = 14,
                Style = Device.Styles.TitleStyle,
            };
            Label lasuupdate  = new Label
            {
                Text = " " + note.Date.ToString(),
                FontSize = 14,
                AutomationId = note.NoteId.ToString(),
                Style = Device.Styles.TitleStyle,
            };

            Switch ispublic = new Switch
            {
                IsToggled = note.isPublic,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                AutomationId = note.NoteId.ToString(),
            };
            ImageButton SaveBut = new ImageButton
            {
                Source = "save.png",
                AutomationId = note.NoteId.ToString(),
            };
            ImageButton DeleBut = new ImageButton
            {
                Source = "delete.png",
                AutomationId = note.NoteId.ToString(),
            };

            Editor text = new Editor
            {
                AutoSize = EditorAutoSizeOption.TextChanges,
                Text = note.Text.ToString() ?? "wot",
                FontSize = 12,
                Style = Device.Styles.BodyStyle,
                AutomationId = note.NoteId.ToString(),
            };

            ClientPage.Switches.Add(ispublic);
            ClientPage.Dates   .Add(lasuupdate);
            ClientPage.Editors .Add(text);

            ispublic.Toggled += OnSwitched;
            SaveBut.Clicked  += OnButtonSaveClicked;
            DeleBut.Clicked  += OnButtonDeleteClicked;

            Grid grid = new Grid
            {
                RowDefinitions =  {
                                     new RowDefinition { Height = new GridLength(30) },
                                     new RowDefinition { Height = new GridLength(20) },
                                     new RowDefinition { Height = new GridLength(1, GridUnitType.Star) } },
                ColumnDefinitions =  {
                                     new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                                     new ColumnDefinition { Width = new GridLength(50) },
                                     new ColumnDefinition { Width = new GridLength(30) },
                                     new ColumnDefinition { Width = new GridLength(30) } },
                ColumnSpacing = 10,
                RowSpacing = 10,
            };

            if (note.RoomId != null) grid.Children.Add(ispublic, 1, 0);
            grid.Children.Add (buildingname, 0, 1);
            grid.Children.Add (lasuupdate, 1, 1);
            grid.Children.Add (roomname, 0, 0);
            grid.Children.Add (SaveBut, 2, 0);
            grid.Children.Add (DeleBut, 3, 0);
            grid.Children.Add (text, 0, 2); 
            Grid.SetColumnSpan(lasuupdate, 2);
            Grid.SetColumnSpan(text, 4);

            return new Frame()
            {
                BorderColor = Color.ForestGreen,
                AutomationId = note.NoteId.ToString(),
                Content = grid,
            };
        }


        public void GoLoginPage(object sender, EventArgs args)
        {
            this.ToolbarItems.Clear();

            if (Client.CurrentClient != null)
            {
                Client.LoginOut();
            }
            ClientPage = null;
            GetLoginPage();
        }


        // Операции с заметкой
        public async void OnButtonSaveClicked(object sender, EventArgs args)
        {
            bool connect = await WebData.CheckConnection();
            if (connect == false) return;

            ImageButton btn = (ImageButton)sender;
            if (btn.AutomationId == "0") { return; }

            Note note = Client.FindNoteById(Int32.Parse(btn.AutomationId));
            if  (note == null) return;

            Note send = (Note)note.Clone(); 
            send.Room = null;

            foreach (var editor in ClientPage.Editors)
            {
                if (editor.AutomationId == btn.AutomationId)
                {
                    send.Text = editor.Text;
                    break;
                }
            }
            foreach (var date   in ClientPage.Dates)
            {
                if (date.AutomationId == btn.AutomationId)
                {
                    send.Date = DateTime.Now.ToString("dd/MM/yyyy");
                    date.Text = send.Date;
                    break;
                }
            }

            Note getnote = await new NoteService().Update(send);

            if (getnote != null) // если обновлена на вервере
            {
                note.Text = send.Text;
                note.Date = send.Date;
                DbService.UpdateNote(note);

                DependencyService.Get<IToast>().Show("Текст заметки обновлен");
                GetClientPage();
                return;
            }
            DependencyService.Get<IToast>().Show("Не удалось сохранить заметку");
        }

        public async void OnButtonDeleteClicked(object sender, EventArgs args)
        {
            bool connect = await WebData.CheckConnection();
            if (connect == false) return;

            ImageButton btn = (ImageButton)sender;
            if (btn.AutomationId == "0") { return; }

            Note note = Client.FindNoteById(Int32.Parse(btn.AutomationId));
            if  (note == null) return;

            Note getnote = await new NoteService().Delete(note.NoteId);

            if (getnote != null) // если удалена с сервера
            {
                DbService.RemoveNote(note);

                DependencyService.Get<IToast>().Show("Заметка о " + note?.RoomName + " удалена");

                // что бы не происходил скролл в начало страницы, обновляю только контент
                List<Frame> v = ((StackLayout)((ScrollView)Content).Content).Children
                                                                   .Where(x => x is Frame)
                                                                   .Select(view => view as Frame)
                                                                   .Where(f => f.AutomationId != btn.AutomationId)
                                                                   .ToList();
                
                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;
                stackLayout.Children.Add(ClientPage.NameLab);
                stackLayout.Children.Add(ClientPage.LoginLab);
                stackLayout.Children.Add(ClientPage.NoteCount);
                foreach (var frame in v)
                {
                    stackLayout.Children.Add(frame);
                }
                stackLayout.Children.Add(ClientPage.LoginOutBtn);
                
                ((ScrollView)Content).Content = stackLayout;

                // GetClientPage();

                return;
            }

            DependencyService.Get<IToast>().Show("Не удалось удалить заметку");
        }

        public async void OnSwitched(object sender, EventArgs args)
        {
            Switch switcher = (Switch)sender;
            if (switcher.AutomationId == "0") { return; }

            bool connect = await WebData.CheckConnection();
            if  (connect == false) { return; }

            Note note = Client.FindNoteById(Int32.Parse(switcher.AutomationId));
            if  (note == null) return;

            Note send = (Note)note.Clone();
            send.Room = null;
            send.isPublic = switcher.IsToggled;
        
            Note getnote = await new NoteService().Update(send);

            if (getnote != null) // если обновлена на вервере
            {
                note.isPublic = send.isPublic;
                DbService.UpdateNote(note);

                if (note.isPublic) { DependencyService.Get<IToast>().Show("Заметка стала публичной"); }
                else { DependencyService.Get<IToast>().Show("Заметка стала приватной"); }

                // GetClientPage();
                return;
            }
        }

        // Toolbar
        public async void OnNewNoteClicked (object sender, EventArgs args)
        {
            await Navigation.PushAsync(new NewNotePage());
        }
        public async void OnFavRoomsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new FavoriteRoomsPage());
        }
    }
}