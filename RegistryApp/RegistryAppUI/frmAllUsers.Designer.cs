namespace RegistryAppUI
{
    partial class frmAllUsers
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.header = new Bunifu.Framework.UI.BunifuCards();
            this.lblModify = new System.Windows.Forms.Label();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.lsvUsers = new System.Windows.Forms.ListView();
            this.UserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDelete = new System.Windows.Forms.Button();
            this.cboAccess = new MetroFramework.Controls.MetroComboBox();
            this.btnChangeType = new System.Windows.Forms.Button();
            this.header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.header.BorderRadius = 5;
            this.header.BottomSahddow = true;
            this.header.color = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(152)))), ((int)(((byte)(215)))));
            this.header.Controls.Add(this.lblModify);
            this.header.Controls.Add(this.btnClose);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.LeftSahddow = false;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.RightSahddow = true;
            this.header.ShadowDepth = 20;
            this.header.Size = new System.Drawing.Size(643, 45);
            this.header.TabIndex = 20;
            // 
            // lblModify
            // 
            this.lblModify.AutoSize = true;
            this.lblModify.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModify.ForeColor = System.Drawing.Color.White;
            this.lblModify.Location = new System.Drawing.Point(11, 17);
            this.lblModify.Name = "lblModify";
            this.lblModify.Size = new System.Drawing.Size(136, 16);
            this.lblModify.TabIndex = 2;
            this.lblModify.Text = "All Registered Users";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnClose.Image = global::RegistryAppUI.Properties.Resources.Shutdown_;
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(605, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(29, 27);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(10, 411);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(623, 10);
            this.panel5.TabIndex = 30;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(633, 45);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 376);
            this.panel4.TabIndex = 31;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 45);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 376);
            this.panel3.TabIndex = 32;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cboAccess);
            this.panel1.Controls.Add(this.btnChangeType);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.lsvUsers);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(623, 366);
            this.panel1.TabIndex = 33;
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this.header;
            this.bunifuDragControl1.Vertical = true;
            // 
            // lsvUsers
            // 
            this.lsvUsers.BackColor = System.Drawing.Color.White;
            this.lsvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.UserName,
            this.columnHeader1});
            this.lsvUsers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lsvUsers.FullRowSelect = true;
            this.lsvUsers.Location = new System.Drawing.Point(0, 63);
            this.lsvUsers.Name = "lsvUsers";
            this.lsvUsers.Size = new System.Drawing.Size(623, 303);
            this.lsvUsers.TabIndex = 0;
            this.lsvUsers.UseCompatibleStateImageBehavior = false;
            this.lsvUsers.View = System.Windows.Forms.View.Details;
            this.lsvUsers.SelectedIndexChanged += new System.EventHandler(this.lsvUsers_SelectedIndexChanged);
            // 
            // UserName
            // 
            this.UserName.Text = "User Name";
            this.UserName.Width = 278;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Access Level";
            this.columnHeader1.Width = 237;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(56)))), ((int)(((byte)(150)))));
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(507, 19);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(107, 29);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cboAccess
            // 
            this.cboAccess.FormattingEnabled = true;
            this.cboAccess.ItemHeight = 23;
            this.cboAccess.Items.AddRange(new object[] {
            "Administrator",
            "User"});
            this.cboAccess.Location = new System.Drawing.Point(6, 19);
            this.cboAccess.Name = "cboAccess";
            this.cboAccess.Size = new System.Drawing.Size(211, 29);
            this.cboAccess.TabIndex = 2;
            this.cboAccess.UseSelectable = true;
            this.cboAccess.SelectedIndexChanged += new System.EventHandler(this.cboAccess_SelectedIndexChanged);
            // 
            // btnChangeType
            // 
            this.btnChangeType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnChangeType.Enabled = false;
            this.btnChangeType.FlatAppearance.BorderSize = 0;
            this.btnChangeType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeType.ForeColor = System.Drawing.Color.White;
            this.btnChangeType.Location = new System.Drawing.Point(233, 19);
            this.btnChangeType.Name = "btnChangeType";
            this.btnChangeType.Size = new System.Drawing.Size(114, 29);
            this.btnChangeType.TabIndex = 1;
            this.btnChangeType.Text = "Change Type";
            this.btnChangeType.UseVisualStyleBackColor = false;
            this.btnChangeType.Click += new System.EventHandler(this.btnChangeType_Click);
            // 
            // frmAllUsers
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.ClientSize = new System.Drawing.Size(643, 421);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.header);
            this.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmAllUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAllUsers";
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCards header;
        private System.Windows.Forms.Label lblModify;
        private Bunifu.Framework.UI.BunifuImageButton btnClose;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private System.Windows.Forms.ListView lsvUsers;
        private MetroFramework.Controls.MetroComboBox cboAccess;
        private System.Windows.Forms.Button btnChangeType;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ColumnHeader UserName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}