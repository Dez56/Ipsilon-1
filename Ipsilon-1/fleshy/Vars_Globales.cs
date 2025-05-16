using Ipsilon_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipsilon_1.fleshy
{
    public static class Vars_Globales
    {
        public static string Uerel => Preferences.Get("direccion_servidor", "");
        public static int UeserID { get; set; }

        public static string ? Nii { get; set; }

        public static Paquete? pask { get; set; }
    }
}
