using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using WinConcursos2.Classes;

namespace WinConcursos2
{
    public partial class Form1 : Form
    {
        ListaCargos listacargosusuario = new ListaCargos();

        public Form1()
        {
            InitializeComponent();

            webBrowser1.Navigate("http://netmasters.com.br/concursos/winconcursos.html?v=" + Application.ProductVersion);

            if (!Directory.Exists(Config.PastaXML))
                Directory.CreateDirectory(Config.PastaXML);

            carregaListaCargosDisco();
            carregaListaCargosUsuarioDisco();

            thListaCargos = new Thread(new ThreadStart(carregaListaCargosOnlineThread));
            thListaCargos.Start();
        }

        private void bAdicionarCargo_Click(object sender, EventArgs e)
        {
            if (cbListaCargos.SelectedIndex >= 0)
            {
                Cargo c = (Cargo)cbListaCargos.SelectedItem;

                int s = 0;

                foreach(TabPage d in tabs.TabPages)
                    if(d.Text.Equals(c.Nome))
                        s++;

                if(s == 0)
                {
                    TabPage t = new TabPage(c.Nome);
                    
                    ucPainelListaConcurso uc = new ucPainelListaConcurso();
                    uc.Dock = DockStyle.Fill;
                    uc.setCargo(c);
                    uc.ConcursoAdicionado += uc_ConcursoAdicionado;
                    t.Controls.Add(uc);
                    tabs.TabPages.Add(t);

                    tabs.SelectedIndex = tabs.TabPages.Count - 1;

                    listacargosusuario.Cargos.Add(c);
                    salvarListaCargosUsuario();
                }
            }
        }

        void uc_ConcursoAdicionado(object sender, EventArgs e)
        {
            int c = (from cont in ((ucPainelListaConcurso)sender).lista.Concursos where cont.Lido == false select cont).Count();

            if (c > 0)
                ((TabPage)((ucPainelListaConcurso)sender).Parent).Text = ((ucPainelListaConcurso)sender).cargo.Nome + " (" + c + ")";
            else
                ((TabPage)((ucPainelListaConcurso)sender).Parent).Text = ((ucPainelListaConcurso)sender).cargo.Nome;
        }
        

        #region Lista de Cargos

        delegate void preencheListaCargosCallback(ListaCargos lista);
        private Thread thListaCargos = null;

        private void preencheListaCargos(ListaCargos lista)
        {
            if (this.cbListaCargos.InvokeRequired)
            {
                preencheListaCargosCallback d = new preencheListaCargosCallback(preencheListaCargos);
                this.Invoke(d, new object[] { lista });
            }
            else
            {
                foreach (Cargo c in lista.Cargos)
                    cbListaCargos.Items.Add(c);
            }
        }
        private void carregaListaCargosOnlineThread()
        {
            ListaCargos cargos = new ListaCargos();

            Cursor.Current = Cursors.WaitCursor;
            
            // Buscando lista no site
            try
            {
                WebRequest req = HttpWebRequest.Create("http://www.pciconcursos.com.br/formacao/");
                req.Method = "GET";

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(req.GetResponse().GetResponseStream(), Encoding.UTF8);

                foreach (HtmlNode c in doc.DocumentNode.SelectNodes("//ul[@class='formacoes']//li//a"))
                    cargos.Cargos.Add(new Cargo() { Nome = c.InnerText, Link = c.GetAttributeValue("href", string.Empty) });

                salvarListaCargos(cargos);
            }
            catch (Exception ex)
            {
                // Erro buscando lista.
                Console.WriteLine(ex.StackTrace);
            }

            preencheListaCargos(cargos);
        }
        private void salvarListaCargos(ListaCargos lista)
        {

            XmlSerializer serializer;
            FileStream fs = null;

            // Salvando XML novo
            try
            {
                if (File.Exists(Config.PastaXML + "\\Cargos.xml"))
                    File.Delete(Config.PastaXML + "\\Cargos.xml");

                serializer = new XmlSerializer(typeof(ListaCargos));
                fs = new FileStream(Config.PastaXML + "\\Cargos.xml", FileMode.OpenOrCreate);
                serializer.Serialize(fs, lista);
                fs.Close();
            }
            catch (Exception ex)
            {
                // Erro salvando xml
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        private void carregaListaCargosDisco()
        {
            ListaCargos cargos = new ListaCargos();

            Cursor.Current = Cursors.WaitCursor;

            XmlSerializer serializer;
            FileStream fs = null;

            // buscando lista no disco
            try
            {
                if (File.Exists(Config.PastaXML + "\\Cargos.xml"))
                {
                    serializer = new XmlSerializer(typeof(ListaCargos));
                    fs = new FileStream(Config.PastaXML + "\\Cargos.xml", FileMode.Open);
                    cargos = (ListaCargos)serializer.Deserialize(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                // Erro buscando lista.
                Console.WriteLine(ex.StackTrace);

            }
            finally
            {
                if (fs != null)
                    fs.Dispose();

            }

            cbListaCargos.Items.Clear();
            foreach (Cargo c in cargos.Cargos)
                cbListaCargos.Items.Add(c);

            Cursor.Current = Cursors.Default;
        }

        #endregion


        #region Lista de Cargos do usuário


        private void salvarListaCargosUsuario()
        {

            XmlSerializer serializer;
            FileStream fs = null;

            // Salvando XML novo
            try
            {
                if (File.Exists(Config.PastaXML + "\\CargosUsuario.xml"))
                    File.Delete(Config.PastaXML + "\\CargosUsuario.xml");

                serializer = new XmlSerializer(typeof(ListaCargos));
                fs = new FileStream(Config.PastaXML + "\\CargosUsuario.xml", FileMode.OpenOrCreate);
                serializer.Serialize(fs, listacargosusuario);
                fs.Close();
            }
            catch (Exception ex)
            {
                // Erro salvando xml
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        private void carregaListaCargosUsuarioDisco()
        {
            Cursor.Current = Cursors.WaitCursor;

            XmlSerializer serializer;
            FileStream fs = null;

            // buscando lista no disco
            try
            {
                if (File.Exists(Config.PastaXML + "\\CargosUsuario.xml"))
                {
                    serializer = new XmlSerializer(typeof(ListaCargos));
                    fs = new FileStream(Config.PastaXML + "\\CargosUsuario.xml", FileMode.Open);
                    listacargosusuario = (ListaCargos)serializer.Deserialize(fs);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                // Erro buscando lista.
                Console.WriteLine(ex.StackTrace);

            }
            finally
            {
                if (fs != null)
                    fs.Dispose();

            }

            foreach (Cargo c in listacargosusuario.Cargos)
            {
                TabPage t = new TabPage(c.Nome);

                ucPainelListaConcurso uc = new ucPainelListaConcurso();
                uc.Dock = DockStyle.Fill;
                uc.setCargo(c);
                uc.ConcursoAdicionado += uc_ConcursoAdicionado;
                t.Controls.Add(uc);
                tabs.TabPages.Add(t);

                tabs.SelectedIndex = tabs.TabPages.Count - 1;
            }

            Cursor.Current = Cursors.Default;
        }

        #endregion

        private void removerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listacargosusuario.Cargos.Remove(((ucPainelListaConcurso)tabs.SelectedTab.Controls[0]).cargo);
            salvarListaCargosUsuario();
            tabs.TabPages.Remove(tabs.SelectedTab);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        public void Notificar(string texto, string titulo)
        {
            notifyIcon1.BalloonTipText = texto;
            notifyIcon1.BalloonTipTitle = titulo;
            notifyIcon1.Icon = new Icon(Resource1.favicon, Resource1.favicon.Size);
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipClicked += notifyIcon1_BalloonTipClicked;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(3600);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Maximized;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Maximized;
        }

    }
}
