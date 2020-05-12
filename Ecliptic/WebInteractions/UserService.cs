﻿using Android.Util;
using Ecliptic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ecliptic.WebInteractions
{
    class UserService
    { 
        const string Url = WebData.ADRESS + "/api/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем пользователя
        public async Task<User> Get(string login, string pass)
        {
            HttpClient client = GetClient();

            HttpResponseMessage response = await client.PostAsync(Url,
                                new StringContent(
                                    JsonConvert.SerializeObject(new { Login = login, Pass = pass }),
                                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }

        // добавляем пользоваетля
        public async Task<Dictionary<string, string>> Register(string name, string login, string pass)
        {
            Dictionary<string, string> user = new Dictionary<string, string>();
            user.Add("Name", name);
            user.Add("Login", login);
            user.Add("Pass", pass);

            HttpClient client = GetClient();
            HttpResponseMessage response = await client.PostAsync(Url + "Register",
                                new StringContent(
                                    JsonConvert.SerializeObject(user),
                                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;    

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
