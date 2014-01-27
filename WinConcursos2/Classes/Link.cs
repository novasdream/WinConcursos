using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WinConcursos2.Classes
{
    [XmlType("Link")]
    public class Link
    {
        [XmlElement("Titulo")]
        public string Titulo { get; set; }

        [XmlElement("ConteudoHTML")]
        public string ConteudoHTML { get; set; }

        [XmlElement("URL")]
        public string URL { get; set; }
    }
}
