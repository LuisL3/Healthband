using System;
using System.Collections.Generic;

namespace Healthband.MenuItems
{
    public class MasterPageItems
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }

        public List<MasterPageItems> Menulist { get; set; }
    }
}