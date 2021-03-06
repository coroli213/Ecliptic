﻿using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    // избранное помещения пользователя
    // можно избавиться от этой сущьности реализовав связь много ко много между client и room (в core версии не так удобно)
    public class FavoriteRoom
    {
        public int FavoriteRoomId { get; set; }

        public string Name     { get; set; }
        public string Building { get; set; }
        public string Details  { get; set; }

        public virtual Client Client { get; set; }
        public virtual int? ClientId { get; set; }


        public FavoriteRoom() { }

        public FavoriteRoom(string name, string details, 
                            string building,
                            int clientid, int roomId)
        {
            Name = name;
            Building = building;

            Details = details;

            ClientId = clientid;
            FavoriteRoomId = 0;
        }
    }
}