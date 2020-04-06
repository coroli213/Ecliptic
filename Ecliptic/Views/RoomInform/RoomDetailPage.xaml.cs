﻿using System;
using System.Linq;
using Xamarin.Forms;
using Ecliptic.Data;
using System.Threading.Tasks;
using Ecliptic.Models;
using Xamarin.Essentials;

namespace Ecliptic.Views
{
    [QueryProperty("Name", "name")]
    public partial class RoomDetailPage : ContentPage
    {
        public string Name
        {
            set
            {
                Current = RoomData.Roooms
                                  .FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));//.Clone();

                Button button1 = null;
                Button button2 = null;
                Label label1 = null;
                Label label2 = null;
                Label label3 = null;
                Label label4 = null;
                Label label5 = null;
                Label label6 = null;

                StackLayout stackLayout = new StackLayout();
                stackLayout.Margin = 20;

                if (Current.Name        != null)
                {
                    label1 = new Label
                    {
                        Text = Current.Name,
                        Style = Device.Styles.TitleStyle,
                        //     FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        //     VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label1);
                }
                if (Current.Description != null)
                {
                    label2 = new Label
                    {
                        Text = Current.Description,
                        FontAttributes = FontAttributes.Italic,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label2);
                }
                if (Current.Details     != null)
                {
                    label3 = new Label
                    {
                        Text = Current.Details,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Fill
                    };

                    stackLayout.Children.Add(label3);
                }
                if (Current             == null)
                {
                    label4 = new Label
                    {
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    stackLayout.Children.Add(label4);
                }
                if (Current.Phone       != null)
                {
                    button1 = new Button
                    {
                        Text = "Call " + Current.Phone,
                    };
                    button1.Clicked += clickphone;

                    stackLayout.Children.Add(button1);
                }
                if (Current.Site        != null)
                {
                    button2 = new Button
                    {
                        Text = "Open web site",
                    };
                    button2.Clicked += clickSite;

                    stackLayout.Children.Add(button2);
                }
                if (Current.Timetable   != null)
                {
                    label5 = new Label
                    {
                        Text = Current.Timetable,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    stackLayout.Children.Add(label5);
                }
                if (Current.Staff       != null)
                {
                    Label labeq = new Label
                    {
                        Text = "Работники:",
                        TextColor = Color.Black,
                        FontSize=12,
                        Style = Device.Styles.TitleStyle,
                        HorizontalOptions = LayoutOptions.Start
                    };
                    stackLayout.Children.Add(labeq);

                    foreach (var i in Current.Staff)
                    {
                        Button buttpers = new Button
                        {
                            Text = i.ToString() ?? "wot",
                            FontSize=12,
                            Style=Device.Styles.TitleStyle,
                        };
                        buttpers.Clicked += clickPers;

                        stackLayout.Children.Add(buttpers);
                    }
                }

                // персональные заметки (только приватные)
                if (!User.isNull)
                {

                    Label labelN = new Label
                    {
                        Text = "Заметки " + User.CurrentUser.Name,
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    stackLayout.Children.Add(labelN);

                    foreach (var i in User.CurrentUser.Notes)
                    {
                        if (i.Room == Current.Name)// && i.isPublic == false)
                        {
                            Grid grid = new Grid
                            {
                                RowDefinitions =  {
                                     new RowDefinition { Height = new GridLength(30) },
                                     new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                                                       },
                                ColumnDefinitions =  {
                                     new ColumnDefinition { Width = new GridLength(160) },
                                     new ColumnDefinition { Width = new GridLength(50) },
                                     new ColumnDefinition { Width = new GridLength(30) },
                                     new ColumnDefinition { Width = new GridLength(30) }
                                                         }
                            };
                            grid.ColumnSpacing = 10;
                            grid.RowSpacing = 10;

                            Label noteLab = new Label
                            {
                                Text = i.Room.ToString() + " Аудитория ",
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

                            grid.Children.Add(noteLab, 0, 0);
                            grid.Children.Add(noteEnt, 0, 1);

                            Grid.SetColumnSpan(noteEnt, 4);

                            Frame frame = new Frame()
                            {
                                BorderColor = Color.ForestGreen,
                                AutomationId = i.Id.ToString(),
                            };

                            frame.Content = grid;

                            stackLayout.Children.Add(frame);
                        }
                    }

                    ToolbarItem item = new ToolbarItem();
                    item.Clicked += OnfaviriteClicked;
                    item.Order = ToolbarItemOrder.Default;
                    item.Priority = 1;
                    if (User.isRoomFavoit(Current))
                    {
                        item.IconImageSource = "@drawable/stared.png";
                    }
                    else
                    {
                        item.IconImageSource = "@drawable/unstared.png";
                    }
                    this.ToolbarItems.Add(item);
                }
                // публичные заметки
                if (NoteData.Notes.Count != 0)
                {
                    Label labelON = new Label
                    {
                        Text = "Общие заметки ",
                        Style = Device.Styles.BodyStyle,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    stackLayout.Children.Add(labelON);

                    foreach (var i in NoteData.Notes)
                    {
                        if (i.Room == Current.Name)
                        {
                            Grid grid = new Grid
                            {
                                RowDefinitions =  {
                                                         new RowDefinition { Height = new GridLength(30) },
                                                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                                                                           },
                                ColumnDefinitions =  {
                                                         new ColumnDefinition { Width = new GridLength(160) },
                                                         new ColumnDefinition { Width = new GridLength(50) },
                                                         new ColumnDefinition { Width = new GridLength(30) },
                                                         new ColumnDefinition { Width = new GridLength(30) }
                                                                             }
                            };
                            grid.ColumnSpacing = 10;
                            grid.RowSpacing = 10;

                            Label noteLab = new Label
                            {
                                Text = i.Room.ToString() + " Аудитория ",
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

                            grid.Children.Add(noteLab, 0, 0);
                            grid.Children.Add(noteEnt, 0, 1);

                            Grid.SetColumnSpan(noteEnt, 4);

                            Frame frame = new Frame()
                            {
                                BorderColor = Color.ForestGreen,
                                AutomationId = i.Id.ToString(),
                            };

                            frame.Content = grid;

                            stackLayout.Children.Add(frame);
                        }
                    }
                }

                ScrollView scrollView = new ScrollView();
                scrollView.Content = stackLayout;

                this.Content = scrollView;

                BindingContext = RoomData.Roooms.FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));
            }
        }

        public Room Current { get; set; }

        public RoomDetailPage()
        {
            InitializeComponent();
        }

        // On open page
        protected override void OnAppearing()
        {
         //   if (User.getInstance() != null)
         //   {
         //       ToolbarItem item = ToolbarItems.Last();
         //
         //       if (User.isRoomFavoit(Current))
         //       {
         //           item.IconImageSource = "@drawable/stared.png";
         //           return;
         //       }
         //       item.IconImageSource = "@drawable/unstared.png";
         //   }
        }

        // Buttons on page
        async void clickphone(object sender, EventArgs args)
        {
            try
            {
                PhoneDialer.Open(Current.Phone);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            //  await DisplayAlert("Alert", "You have 1 been alerted"+Current.Phone , "OK");
        }
        async void clickSite(object sender, EventArgs args)
        {
            new System.Threading.Thread(() =>
            {
                Launcher.OpenAsync(new Uri(Current.Site));
                //Device.OpenUri(new Uri(Current.Site));
            }).Start();
        }
        async void clickPers(object sender, EventArgs args)
        {
            Button btn = (Button)sender;

            string[] words = btn.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            
            //DisplayAlert("Alert", words[2] ?? "no", "OK");

            await Navigation.PushAsync(new WorkerDetailPage(words));
        }

        // Star click
        async void OnfaviriteClicked(object sender, EventArgs args)
        {
            if (User.getInstance() != null)
            {
                ToolbarItem item = (ToolbarItem)sender;

                if (User.isRoomFavoit(Current))
                {
                    ToolbarItems.Last().IconImageSource = "@drawable/unstared.png";
                    User.CurrentUser.Favorites.Remove(Current);
                }
                else
                {
                    ToolbarItems.Last().IconImageSource = "@drawable/stared.png";
                    User.CurrentUser.Favorites.Add(Current);
                }
            }
        }

        // Toolbar
        async void OnButton1Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 1 been alerted", "OK");
        }
        async void OnButton2Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 2 been alerted", "OK");
        }
        async void OnButton3Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 3 been alerted", "OK");
        }
        async void OnButton4Clicked(object sender, EventArgs args)
        {
            await DisplayAlert("Alert", "You have 4 been alerted", "OK");
        }
    }
}