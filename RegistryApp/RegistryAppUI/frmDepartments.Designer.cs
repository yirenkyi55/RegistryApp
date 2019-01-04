namespace RegistryAppUI
{
    partial class frmDepartments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDepartments));
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.header = new Bunifu.Framework.UI.BunifuCards();
            this.bunifuCards2 = new Bunifu.Framework.UI.BunifuCards();
            this.bunifuCards3 = new Bunifu.Framework.UI.BunifuCards();
            this.label1 = new System.Windows.Forms.Label();
            this.indicator = new Bunifu.Framework.UI.BunifuSeparator();
            this.btnDepartments = new Bunifu.Framework.UI.BunifuThinButton2();
            this.btnCreateNew = new Bunifu.Framework.UI.BunifuThinButton2();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.departmentList1 = new RegistryAppUI.UserControls.DepartmentList();
            this.newDepartment1 = new RegistryAppUI.UserControls.NewDepartment();
            this.header.SuspendLayout();
            this.bunifuCards2.SuspendLayout();
            this.bunifuCards3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this.header;
            this.bunifuDragControl1.Vertical = true;
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.header.BorderRadius = 5;
            this.header.BottomSahddow = true;
            this.header.color = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(152)))), ((int)(((byte)(215)))));
            this.header.Controls.Add(this.label1);
            this.header.Controls.Add(this.btnClose);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.LeftSahddow = false;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.RightSahddow = true;
            this.header.ShadowDepth = 20;
            this.header.Size = new System.Drawing.Size(693, 47);
            this.header.TabIndex = 0;
            // 
            // bunifuCards2
            // 
            this.bunifuCards2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.bunifuCards2.BorderRadius = 0;
            this.bunifuCards2.BottomSahddow = true;
            this.bunifuCards2.color = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.bunifuCards2.Controls.Add(this.indicator);
            this.bunifuCards2.Controls.Add(this.btnDepartments);
            this.bunifuCards2.Controls.Add(this.btnCreateNew);
            this.bunifuCards2.Dock = System.Windows.Forms.DockStyle.Left;
            this.bunifuCards2.LeftSahddow = false;
            this.bunifuCards2.Location = new System.Drawing.Point(0, 47);
            this.bunifuCards2.Name = "bunifuCards2";
            this.bunifuCards2.RightSahddow = true;
            this.bunifuCards2.ShadowDepth = 20;
            this.bunifuCards2.Size = new System.Drawing.Size(165, 426);
            this.bunifuCards2.TabIndex = 0;
            // 
            // bunifuCards3
            // 
            this.bunifuCards3.BackColor = System.Drawing.Color.White;
            this.bunifuCards3.BorderRadius = 0;
            this.bunifuCards3.BottomSahddow = true;
            this.bunifuCards3.color = System.Drawing.Color.White;
            this.bunifuCards3.Controls.Add(this.departmentList1);
            this.bunifuCards3.Controls.Add(this.newDepartment1);
            this.bunifuCards3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bunifuCards3.LeftSahddow = false;
            this.bunifuCards3.Location = new System.Drawing.Point(165, 47);
            this.bunifuCards3.Name = "bunifuCards3";
            this.bunifuCards3.RightSahddow = true;
            this.bunifuCards3.ShadowDepth = 20;
            this.bunifuCards3.Size = new System.Drawing.Size(528, 426);
            this.bunifuCards3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Departments";
            // 
            // indicator
            // 
            this.indicator.BackColor = System.Drawing.Color.Transparent;
            this.indicator.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(56)))), ((int)(((byte)(150)))));
            this.indicator.LineThickness = 5;
            this.indicator.Location = new System.Drawing.Point(-1, 45);
            this.indicator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.indicator.Name = "indicator";
            this.indicator.Size = new System.Drawing.Size(17, 45);
            this.indicator.TabIndex = 1;
            this.indicator.Transparency = 255;
            this.indicator.Vertical = true;
            // 
            // btnDepartments
            // 
            this.btnDepartments.ActiveBorderThickness = 1;
            this.btnDepartments.ActiveCornerRadius = 1;
            this.btnDepartments.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnDepartments.ActiveForecolor = System.Drawing.Color.Gainsboro;
            this.btnDepartments.ActiveLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnDepartments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.btnDepartments.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDepartments.BackgroundImage")));
            this.btnDepartments.ButtonText = "Departments";
            this.btnDepartments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDepartments.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDepartments.ForeColor = System.Drawing.Color.SeaGreen;
            this.btnDepartments.IdleBorderThickness = 1;
            this.btnDepartments.IdleCornerRadius = 1;
            this.btnDepartments.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.btnDepartments.IdleForecolor = System.Drawing.Color.White;
            this.btnDepartments.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.btnDepartments.Location = new System.Drawing.Point(22, 125);
            this.btnDepartments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDepartments.Name = "btnDepartments";
            this.btnDepartments.Size = new System.Drawing.Size(143, 45);
            this.btnDepartments.TabIndex = 1;
            this.btnDepartments.Tag = "0, 122, 204";
            this.btnDepartments.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDepartments.Click += new System.EventHandler(this.btnDepartments_Click_1);
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.ActiveBorderThickness = 1;
            this.btnCreateNew.ActiveCornerRadius = 1;
            this.btnCreateNew.ActiveFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnCreateNew.ActiveForecolor = System.Drawing.Color.Gainsboro;
            this.btnCreateNew.ActiveLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnCreateNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.btnCreateNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreateNew.BackgroundImage")));
            this.btnCreateNew.ButtonText = "Create New";
            this.btnCreateNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateNew.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateNew.ForeColor = System.Drawing.Color.SeaGreen;
            this.btnCreateNew.IdleBorderThickness = 1;
            this.btnCreateNew.IdleCornerRadius = 1;
            this.btnCreateNew.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.btnCreateNew.IdleForecolor = System.Drawing.Color.White;
            this.btnCreateNew.IdleLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.btnCreateNew.Location = new System.Drawing.Point(22, 45);
            this.btnCreateNew.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(143, 45);
            this.btnCreateNew.TabIndex = 1;
            this.btnCreateNew.Tag = "0, 122, 204";
            this.btnCreateNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnClose.Image = global::RegistryAppUI.Properties.Resources.Shutdown_;
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(650, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(36, 29);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // departmentList1
            // 
            this.departmentList1.BackColor = System.Drawing.Color.White;
            this.departmentList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.departmentList1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departmentList1.Location = new System.Drawing.Point(0, 0);
            this.departmentList1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.departmentList1.Name = "departmentList1";
            this.departmentList1.Size = new System.Drawing.Size(528, 426);
            this.departmentList1.TabIndex = 2;
            // 
            // newDepartment1
            // 
            this.newDepartment1.BackColor = System.Drawing.Color.White;
            this.newDepartment1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newDepartment1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newDepartment1.Location = new System.Drawing.Point(0, 0);
            this.newDepartment1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.newDepartment1.Name = "newDepartment1";
            this.newDepartment1.Size = new System.Drawing.Size(528, 426);
            this.newDepartment1.TabIndex = 1;
            // 
            // frmDepartments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(50)))), ((int)(((byte)(71)))));
            this.ClientSize = new System.Drawing.Size(693, 473);
            this.Controls.Add(this.bunifuCards3);
            this.Controls.Add(this.bunifuCards2);
            this.Controls.Add(this.header);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmDepartments";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDepartments";
            this.Load += new System.EventHandler(this.frmDepartments_Load);
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.bunifuCards2.ResumeLayout(false);
            this.bunifuCards3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private Bunifu.Framework.UI.BunifuCards bunifuCards3;
        private Bunifu.Framework.UI.BunifuCards bunifuCards2;
        private Bunifu.Framework.UI.BunifuCards header;
        private Bunifu.Framework.UI.BunifuImageButton btnClose;
        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuThinButton2 btnCreateNew;
        private Bunifu.Framework.UI.BunifuSeparator indicator;
        private Bunifu.Framework.UI.BunifuThinButton2 btnDepartments;
        private UserControls.NewDepartment newDepartment1;
        private UserControls.DepartmentList departmentList1;
    }
}