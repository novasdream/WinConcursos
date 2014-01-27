using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WinConcursos2.Classes
{
    [XmlRoot("ListaConcursos")]
    public class ListaConcursos
    {
        [XmlArray("concursos")]
        [XmlArrayItem("Concurso")]
        public List<Concurso> Concursos { get; set; }

        public ListaConcursos()
        {
            Concursos = new List<Concurso>();
        }
    }
}
