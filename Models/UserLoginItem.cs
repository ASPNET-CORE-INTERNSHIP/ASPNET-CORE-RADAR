﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class UserLoginItem
    {
        public long Id { get; set; }

        public string Username { get; set; }
        public string Usermail { get; set; }
        public string Userpassword { get; set; }

        //0 - Request has been send
        //1 - Sucessfully logged in
        //2 - Password not correct
        //3 - User not found
        public int isUserLoggedIn { get; set; }
    }
}
