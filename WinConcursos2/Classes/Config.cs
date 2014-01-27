using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinConcursos2.Classes
{
    static public class Config
    {
        static public string PastaXML { get { return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\WinConcursos\\XML"; } }
    }
}
