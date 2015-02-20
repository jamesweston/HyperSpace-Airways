using System;
using System.Collections.Generic;
using System.Text;

namespace HyperSpace_Airways
{
    public static class General
    {
        private static bool masterswitch = true;
        public static bool MasterSwitch
        {
            get
            {
                return masterswitch;
            }
            set
            {
                masterswitch = value;
            }
    }
}
}
