using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WinConcursos2.Classes
{
    [XmlType("Cargo")]
    public class Cargo
    {
        [XmlElement("Nome")]
        public string Nome { get; set; }

        [XmlElement("Link")]
        public string Link { get; set; }

        override public string ToString()
        {
            return Nome;
        }
    }
}
