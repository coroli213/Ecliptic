﻿using System.Linq;
using Xamarin.Forms;
using Ecliptic.Models;
using System.Collections.ObjectModel;
using Ecliptic.Data;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using Ecliptic.Views.WayFounder;

namespace Ecliptic.Views
{
    public partial class FindWayPage : ContentPage
    {
        public FindWayPage()
        {
            InitializeComponent();
            stackBar1.HeightRequest = 1;
            stackBar2.HeightRequest = 1;
        }

        // установить начальные значения, если уже установлены помещения
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Way.Begin != null)
            {
                taped = true; // установить, что бы установки имяни не произошел OnTextFromChanged
                searchBar1.Text = Way.Begin.Name;
                stackBar1.HeightRequest = 1;
                searchResults1.ItemsSource = new List<Room>();
            }
            if (Way.End != null)
            {
                taped = true; // установить, что бы установки имяни не произошел OnTextToChanged
                searchBar2.Text = Way.End?.Name;
                stackBar2.HeightRequest = 1;
                searchResults2.ItemsSource = new List<Room>();
            }
        }

        #region TextChanged
        void OnTextFromChanged(object sender, EventArgs e)
        {
            if (taped == false)
            {
                Way.Begin = null; // обнулить начало если изменение текста произошло не по выбору конкретного элемента
            }

            if (searchBar1.Text == "") // если текста нет - очистить список
            {
                searchResults1.ItemsSource = new List<Room>();
                stackBar1.HeightRequest = 1;
                return;
            }

            // иначе найти помещения, подходящие под описание
            var searchedrooms = RoomData.Rooms.Where(room => room.Name.ToLower().Contains(searchBar1.Text.ToLower()) ||
                                                     room .Description.ToLower().Contains(searchBar1.Text.ToLower())).ToList<Room>();

            // если помещение одно, установить его как начало
            if (searchedrooms.Count == 1)
            {
                Way.Begin = searchedrooms.First();
            }

            // вывести результаты в список
            stackBar1.HeightRequest    = searchedrooms.Count() > 10 ? 200 : searchedrooms.Count() * 50;
            searchResults1.ItemsSource = searchedrooms;
            taped = false;
        }
        void OnTextToChanged(object sender, EventArgs e)
        {
            if (taped == false)
            {
                Way.End = null; // обнулить конец если изменение текста произошло не по выбору конкретного элемента
            }

            if (searchBar2.Text == "") // если текста нет - очистить список
            {
                searchResults2.ItemsSource = new List<Room>();
                stackBar2.HeightRequest = 1;
                return;
            }

            // иначе найти помещения, подходящие под описание
            var searchedrooms = RoomData.Rooms.Where(room => room.Name.ToLower().Contains(searchBar2.Text.ToLower()) ||
                                                     room.Description.ToLower().Contains(searchBar2.Text.ToLower())).ToList<Room>();

            // если помещение одно, установить его как начало
            if (searchedrooms.Count == 1)
            {
                Way.End = searchedrooms.First();
            }

            // вывести результаты в список
            stackBar2.HeightRequest = searchedrooms.Count() > 10 ? 200 : searchedrooms.Count() * 50;
            searchResults2.ItemsSource = searchedrooms;
            taped = false;
        }
        #endregion

        #region ItemTapped
        public bool taped   = false; // было ли последним действием выбор элемента из списка 

        // выбор элемента из списка устанавливает его как начало/конец маршрута
        private void OnRoomFromTapped(object sender, ItemTappedEventArgs e)
        {
            taped = true;
            Room room = (e.Item as Room);
            Way.Begin = room;
            searchBar1.Text = room.Name;
            stackBar1.HeightRequest = 1;
            searchResults1.ItemsSource = new List<Room>();
        }
        private void OnRoomToTapped  (object sender, ItemTappedEventArgs e)
        {
            taped = true;
            Room room = (e.Item as Room); 
            Way.End = room;
            searchBar2.Text = room.Name;
            stackBar2.HeightRequest = 1;
            searchResults2.ItemsSource = new List<Room>();
        }
        #endregion
        
        private void Button_Clicked  (object sender, EventArgs e)
        {
            taped = true;
            if (RoomData.Rooms.Count == 0) { DependencyService.Get<IToast>().Show("Помещения не загружены"); return; }

            if (searchBar1.Text == null || searchBar1.Text == "") { DependencyService.Get<IToast>().Show("Начало маршрута не задано"); return; }
            if (Way.Begin == null) { DependencyService.Get<IToast>().Show("Начало маршрута неоднозначно"); return; }

            if (searchBar2.Text == null || searchBar2.Text == "") { DependencyService.Get<IToast>().Show("Конец маршрута не задан");   return; }
            if (Way.End   == null) { DependencyService.Get<IToast>().Show("Конец маршрута неоднозначен");  return; }

            if (Way.End.Equals(Way.Begin)) { DependencyService.Get<IToast>().Show("Вы находитесь в месте назначения"); return; }

            // если все окей - производить поиск
            List<PointM> path;
            try
            {
                 path = new Dijkstra().FindShortestPath(PointData.Find(Way.Begin),
                                                        PointData.Find(Way.End));
            }
            catch(NullReferenceException ex)
            {
                DependencyService.Get<IToast>().Show("Помещения не связаны");
                return;
            }

            // преобразовать список точек в список ребер маршрута
            EdgeData.ConvertPathToWay(path);

            // установить длину маршрута
            double way = 0;
            foreach(var w in EdgeData.Ways) { way += w.Weight; }
            WayLarge.Text = "Длина пути " +   way + " метов";
            EndWayButton.IsVisible = true;

            taped = false;
            DependencyService.Get<IToast>().Show("Маршрут установлен");
        }

        // очистить маршрут
        private void EndWay_Clicked(object sender, EventArgs e)
        {
            EdgeData.Ways= new List<EdgeM>();
            WayLarge.Text = " ";
            searchBar1.Text = "";
            searchBar2.Text = "";
            EndWayButton.IsVisible = false;
        }
    }
}