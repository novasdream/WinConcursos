namespace WinConcursos2
{
    partial class ucPainelListaConcurso
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbConcursos = new System.Windows.Forms.ListView();
            this.Concurso = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabsControle = new System.Windows.Forms.TabControl();
            this.tabConteudo = new System.Windows.Forms.TabPage();
            this.wbConteudo = new System.Windows.Forms.WebBrowser();
            this.bLido = new System.Windows.Forms.Button();
            this.tabsControle.SuspendLayout();
            this.tabConteudo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbConcursos
            // 
            this.lbConcursos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbConcursos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Concurso});
            this.lbConcursos.FullRowSelect = true;
            this.lbConcursos.Location = new System.Drawing.Point(3, 32);
            this.lbConcursos.MultiSelect = false;
            this.lbConcursos.Name = "lbConcursos";
            this.lbConcursos.Size = new System.Drawing.Size(333, 535);
            this.lbConcursos.TabIndex = 7;
            this.lbConcursos.UseCompatibleStateImageBehavior = false;
            this.lbConcursos.View = System.Windows.Forms.View.Details;
            this.lbConcursos.SelectedIndexChanged += new System.EventHandler(this.lbConcursos_SelectedIndexChanged);
            // 
            // Concurso
            // 
            this.Concurso.Text = "Concurso";
            this.Concurso.Width = 325;
            // 
            // tabsControle
            // 
            this.tabsControle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsControle.Controls.Add(this.tabConteudo);
            this.tabsControle.Location = new System.Drawing.Point(342, 32);
            this.tabsControle.Name = "tabsControle";
            this.tabsControle.SelectedIndex = 0;
            this.tabsControle.Size = new System.Drawing.Size(809, 535);
            this.tabsControle.TabIndex = 6;
            // 
            // tabConteudo
            // 
            this.tabConteudo.Controls.Add(this.wbConteudo);
            this.tabConteudo.Location = new System.Drawing.Point(4, 22);
            this.tabConteudo.Name = "tabConteudo";
            this.tabConteudo.Padding = new System.Windows.Forms.Padding(3);
            this.tabConteudo.Size = new System.Drawing.Size(801, 509);
            this.tabConteudo.TabIndex = 0;
            this.tabConteudo.Text = "CONTEÚDO";
            this.tabConteudo.UseVisualStyleBackColor = true;
            // 
            // wbConteudo
            // 
            this.wbConteudo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbConteudo.Location = new System.Drawing.Point(3, 3);
            this.wbConteudo.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbConteudo.Name = "wbConteudo";
            this.wbConteudo.Size = new System.Drawing.Size(795, 503);
            this.wbConteudo.TabIndex = 1;
            // 
            // bLido
            // 
            this.bLido.Location = new System.Drawing.Point(3, 3);
            this.bLido.Name = "bLido";
            this.bLido.Size = new System.Drawing.Size(138, 23);
            this.bLido.TabIndex = 8;
            this.bLido.Text = "Marcar tudo como lido";
            this.bLido.UseVisualStyleBackColor = true;
            this.bLido.Click += new System.EventHandler(this.bLido_Click);
            // 
            // ucPainelListaConcurso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bLido);
            this.Controls.Add(this.lbConcursos);
            this.Controls.Add(this.tabsControle);
            this.Name = "ucPainelListaConcurso";
            this.Size = new System.Drawing.Size(1154, 570);
            this.tabsControle.ResumeLayout(false);
            this.tabConteudo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lbConcursos;
        private System.Windows.Forms.ColumnHeader Concurso;
        private System.Windows.Forms.TabControl tabsControle;
        private System.Windows.Forms.TabPage tabConteudo;
        private System.Windows.Forms.WebBrowser wbConteudo;
        private System.Windows.Forms.Button bLido;
    }
}
