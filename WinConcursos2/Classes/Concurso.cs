using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WinConcursos2.Classes
{
    [XmlType("Concurso")]
    public class Concurso
    {
        [XmlElement("Titulo")]
        public string Titulo { get; set; }

        [XmlElement("Lugar")]
        public string Lugar { get; set; }

        [XmlElement("Link")]
        public string Link { get; set; }

        [XmlElement("ConteudoHTML")]
        public string ConteudoHTML { get; set; }

        [XmlArray("Links")]
        [XmlArrayItem("Links")]
        public List<Link> Links { get; set; }

        [XmlElement("Data")]
        public string Data { get; set; }

        [XmlElement("Lido")]
        public bool Lido { get; set; }

        public Concurso()
        {
            Links = new List<Link>();
        }

        override public string ToString()
        {
            return Lugar;
        }
    }
}
