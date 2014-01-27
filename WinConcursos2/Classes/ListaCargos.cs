using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WinConcursos2.Classes
{
    [XmlRoot("ListaCargos")]
    public class ListaCargos
    {
        [XmlArray("Cargos")]
        [XmlArrayItem("Cargo")]
        public List<Cargo> Cargos { get; set; }

        public ListaCargos()
        {
            Cargos = new List<Cargo>();
        }
    }
}
