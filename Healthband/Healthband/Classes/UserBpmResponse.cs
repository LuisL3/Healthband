using System;
using System.Collections.Generic;
using System.Text;

namespace Healthband.Classes
{
    class UserBpmResponse
    {
        public int ID_BPM { get; set; }
        public int VALUE_BPM { get; set; }
        public string DATE_BPM { get; set; }
        public string TIME_BPM { get; set; }
        public int ID_user { get; set; }


        public UserBpmResponse(int _id_bpm, int _value_bpm, string _date, string _time, int _id_user)
        {
            ID_BPM = _id_bpm;
            VALUE_BPM = _value_bpm;
            DATE_BPM = _date;
            TIME_BPM = _time;
            ID_user = _id_user;
        }

    }
}
