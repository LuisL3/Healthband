using System;
using System.Collections.Generic;
using System.Text;

namespace Healthband.Classes
{
    class UserLoginResponse
    {
        public string Response { get; set; }
        public int Id_user { get; set; }

        public UserLoginResponse(string response, int id_user)
        {
            Response = response;
            Id_user = id_user;
        }
    }
}
