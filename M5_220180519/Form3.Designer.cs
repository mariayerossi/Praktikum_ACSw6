
namespace M5_220180519
{
    partial class Form3
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
            this.btnRoti = new System.Windows.Forms.Button();
            this.btnKaryawan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRoti
            // 
            this.btnRoti.Location = new System.Drawing.Point(84, 105);
            this.btnRoti.Name = "btnRoti";
            this.btnRoti.Size = new System.Drawing.Size(169, 54);
            this.btnRoti.TabIndex = 0;
            this.btnRoti.Text = "Master Roti";
            this.btnRoti.UseVisualStyleBackColor = true;
            this.btnRoti.Click += new System.EventHandler(this.btnRoti_Click);
            // 
            // btnKaryawan
            // 
            this.btnKaryawan.Location = new System.Drawing.Point(84, 223);
            this.btnKaryawan.Name = "btnKaryawan";
            this.btnKaryawan.Size = new System.Drawing.Size(169, 54);
            this.btnKaryawan.TabIndex = 1;
            this.btnKaryawan.Text = "Master Karyawan";
            this.btnKaryawan.UseVisualStyleBackColor = true;
            this.btnKaryawan.Click += new System.EventHandler(this.btnKaryawan_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 390);
            this.Controls.Add(this.btnKaryawan);
            this.Controls.Add(this.btnRoti);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRoti;
        private System.Windows.Forms.Button btnKaryawan;
    }
}