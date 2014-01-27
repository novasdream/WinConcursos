using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinConcursos2.Classes;
using System.Net;
using HtmlAgilityPack;
using System.Threading;
using System.IO;
using System.Xml.Serialization;

namespace WinConcursos2
{
    public partial class ucPainelListaConcurso : UserControl
    {
        public Cargo cargo;
        public ListaConcursos lista = new ListaConcursos();
        private Thread thPreencheLista = null;

        public ucPainelListaConcurso()
        {
            InitializeComponent();
        }

        public void setCargo(Cargo cargo)
        {
            this.cargo = cargo;
            carregaListaConcursos();

            if (ConcursoAdicionado != null)
                ConcursoAdicionado(this, new EventArgs());

            thPreencheLista = new Thread(new ThreadStart(atualizaListaConcursosThread));
            thPreencheLista.Start();
        }

        protected HtmlNode getH3Anterior(HtmlNode h)
        {
            return (h.PreviousSibling.Name.Equals("h3")) ? h.PreviousSibling : getH3Anterior(h.PreviousSibling);
        }

        protected void atualizaListaConcursosThread()
        {
            System.Threading.Thread.Sleep(2000);
            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                int added = 0;
                WebRequest req = HttpWebRequest.Create(cargo.Link);
                req.Method = "GET";

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(req.GetResponse().GetResponseStream(), Encoding.UTF8);

                foreach (HtmlNode c in doc.DocumentNode.SelectNodes("//ul[@class='concursos']//li//a"))
                {
                    HtmlNode h3 = getH3Anterior(c.ParentNode.ParentNode);

                    Concurso con = new Concurso()
                    {
                        Titulo = c.InnerText,
                        Lugar = h3.InnerText,
                        Link = h3.FirstChild.GetAttributeValue("href", string.Empty),
                        Data = DateTime.Now.ToShortDateString(),
                        Lido = false
                    };

                    int cont = (from h in lista.Concursos where h.Link.Equals(con.Link) select h).Count();

                    if (cont == 0)
                    {
                        lista.Concursos.Add(con);
                        added++;
                    }
                }

                if(added > 0)
                    salvar();

                preencheLista(lista);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                //statusStrip1.Invoke((MethodInvoker)(() => statusMensagem1.Text = "Não foi possível atualizar... =("));
            }
        }


        protected void carregaListaConcursos()
        {
            if (File.Exists(Config.PastaXML + "\\" + cargo.Nome + ".xml"))
            {
                FileStream fs = null;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    XmlSerializer serializer = new XmlSerializer(typeof(ListaConcursos));
                    fs = new FileStream(Config.PastaXML + "\\" + cargo.Nome + ".xml", FileMode.Open);
                    lista = (ListaConcursos)serializer.Deserialize(fs);
                    fs.Close();

                    preencheLista(lista);
                }
                catch (Exception ex)
                {

                }
                finally 
                {
                    if (fs != null)
                        fs.Dispose();
                }
            }
        }

        delegate void preencheListaCallback(ListaConcursos lista);
        protected void preencheLista(ListaConcursos lista)
        {
            if (this.lbConcursos.InvokeRequired)
            {
                preencheListaCallback d = new preencheListaCallback(preencheLista);
                this.Invoke(d, new object[] { lista });
            }
            else
            {

                lbConcursos.Items.Clear();

                int added = 0;

                foreach (Concurso concur in lista.Concursos)
                {
                    ListViewItem lvi = new ListViewItem(concur.Lugar);

                    if (concur.Data.Equals(DateTime.Now.ToShortDateString()))
                        lvi.ForeColor = Color.Blue;

                    if (!concur.Lido && string.IsNullOrEmpty(concur.ConteudoHTML))
                    {
                        lvi.Font = new Font(lbConcursos.Font, FontStyle.Bold);
                        added++;
                    }

                    lbConcursos.Items.Insert(0, lvi);
                    
                }

                if (added > 0)
                {
                    if (Form1.ActiveForm != null)
                        ((Form1)Form1.ActiveForm).Notificar("Você tem " + added + " novos concursos adicionados.\nClique aqui para abrir!", "Novos concursos!");
                    
                    if (ConcursoAdicionado != null)
                        ConcursoAdicionado(this, new EventArgs());
                }
                
            }
        }

        public event EventHandler ConcursoAdicionado;

        protected void salvar()
        {
            FileStream fs = null;
            try
            {
                if (File.Exists(Config.PastaXML + "\\" + cargo.Nome + ".xml"))
                    File.Delete(Config.PastaXML + "\\" + cargo.Nome + ".xml");

                XmlSerializer serializer = new XmlSerializer(typeof(ListaConcursos));
                fs = new FileStream(Config.PastaXML + "\\" + cargo.Nome + ".xml", FileMode.OpenOrCreate);
                serializer.Serialize(fs, lista);
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        private void lbConcursos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbConcursos.SelectedItems.Count > 0 && lbConcursos.SelectedItems[0].Index >= 0)
            {
                Concurso c = lista.Concursos[
                    lista.Concursos.Count - 1 -
                    lbConcursos.SelectedItems[0].Index
                    ];

                while (tabsControle.TabCount > 1)
                {
                    foreach (Control cc in tabsControle.TabPages[tabsControle.TabCount - 1].Controls)
                        cc.Dispose();

                    tabsControle.TabPages[tabsControle.TabCount - 1].Dispose();
                }

                if (string.IsNullOrEmpty(c.ConteudoHTML))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Thread t3 = new Thread(new ParameterizedThreadStart(carregaDados));
                    t3.Start(c);
                    while (t3.IsAlive) ;

                    if (ConcursoAdicionado != null)
                        ConcursoAdicionado(this, new EventArgs());
                }

                wbConteudo.DocumentText = c.ConteudoHTML;

                foreach (Link l in c.Links)
                {
                    TabPage tab = new TabPage(l.Titulo);
                    WebBrowser br = new WebBrowser();
                    br.Dock = DockStyle.Fill;

                    if (string.IsNullOrEmpty(l.ConteudoHTML))
                        br.DocumentText = "Vish, não achei! Mas você pode tentar <a href='" + l.URL + "'>clicar aqui para baixar</a>! ;-)";
                    else
                        br.DocumentText = l.ConteudoHTML;

                    tab.Controls.Add(br);
                    tabsControle.TabPages.Add(tab);
                }

                lbConcursos.SelectedItems[0].Font = new Font(lbConcursos.Font, FontStyle.Regular);
                c = null;
            }
        }

        private void carregaDados(object con)
        {
            WebRequest req = HttpWebRequest.Create(((Concurso)con).Link);
            req.Method = "GET";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(req.GetResponse().GetResponseStream(), Encoding.UTF8);

            HtmlNode not = doc.DocumentNode.SelectSingleNode("//div[@id='noticia']");
            if (not != null)
                ((Concurso)con).ConteudoHTML = not.InnerHtml;

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//div[@id='links']//ul//li//a"))
            {
                if (!link.InnerText.Equals("PROVAS RELACIONADAS"))
                {
                    WebRequest reqe = HttpWebRequest.Create(link.GetAttributeValue("href", string.Empty));
                    reqe.Method = "GET";
                    HtmlAgilityPack.HtmlDocument doce = new HtmlAgilityPack.HtmlDocument();
                    doce.Load(reqe.GetResponse().GetResponseStream(), Encoding.UTF8);

                    HtmlNode edital = doce.DocumentNode.SelectSingleNode("//div[@id='edital']");

                    ((Concurso)con).Links.Add(
                        new Link()
                        {
                            URL = link.GetAttributeValue("href", string.Empty),
                            Titulo = link.InnerText,
                            ConteudoHTML = (edital != null) ? edital.InnerHtml : string.Empty
                        }
                    );
                }
            }

            ((Concurso)con).Lido = true;

            salvar();
        }

        private void bLido_Click(object sender, EventArgs e)
        {
            foreach (Concurso c in lista.Concursos)
                c.Lido = true;

            foreach (ListViewItem l in lbConcursos.Items)
                l.Font = new Font(lbConcursos.Font, FontStyle.Regular);

            salvar();

            if (ConcursoAdicionado != null)
                ConcursoAdicionado(this, new EventArgs());
        }

        
    }
}
