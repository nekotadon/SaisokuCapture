namespace SaisokuCapture
{
    partial class FormSelectedArea
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label_explain = new System.Windows.Forms.Label();
            this.button_set1 = new System.Windows.Forms.Button();
            this.button_set2 = new System.Windows.Forms.Button();
            this.button_set3 = new System.Windows.Forms.Button();
            this.button_set4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox.Location = new System.Drawing.Point(108, 107);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(354, 166);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // label_explain
            // 
            this.label_explain.AutoSize = true;
            this.label_explain.Location = new System.Drawing.Point(248, 17);
            this.label_explain.Name = "label_explain";
            this.label_explain.Size = new System.Drawing.Size(117, 12);
            this.label_explain.TabIndex = 1;
            this.label_explain.Text = "↓この枠内が選択範囲";
            // 
            // button_set1
            // 
            this.button_set1.Location = new System.Drawing.Point(12, 12);
            this.button_set1.Name = "button_set1";
            this.button_set1.Size = new System.Drawing.Size(54, 23);
            this.button_set1.TabIndex = 2;
            this.button_set1.Text = "設定";
            this.button_set1.UseVisualStyleBackColor = true;
            this.button_set1.Click += new System.EventHandler(this.button_set_Click);
            // 
            // button_set2
            // 
            this.button_set2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_set2.Location = new System.Drawing.Point(549, 12);
            this.button_set2.Name = "button_set2";
            this.button_set2.Size = new System.Drawing.Size(54, 23);
            this.button_set2.TabIndex = 2;
            this.button_set2.Text = "設定";
            this.button_set2.UseVisualStyleBackColor = true;
            this.button_set2.Click += new System.EventHandler(this.button_set_Click);
            // 
            // button_set3
            // 
            this.button_set3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_set3.Location = new System.Drawing.Point(12, 332);
            this.button_set3.Name = "button_set3";
            this.button_set3.Size = new System.Drawing.Size(54, 23);
            this.button_set3.TabIndex = 2;
            this.button_set3.Text = "設定";
            this.button_set3.UseVisualStyleBackColor = true;
            this.button_set3.Click += new System.EventHandler(this.button_set_Click);
            // 
            // button_set4
            // 
            this.button_set4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_set4.Location = new System.Drawing.Point(549, 332);
            this.button_set4.Name = "button_set4";
            this.button_set4.Size = new System.Drawing.Size(54, 23);
            this.button_set4.TabIndex = 2;
            this.button_set4.Text = "設定";
            this.button_set4.UseVisualStyleBackColor = true;
            this.button_set4.Click += new System.EventHandler(this.button_set_Click);
            // 
            // FormSelectedArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 367);
            this.Controls.Add(this.button_set4);
            this.Controls.Add(this.button_set3);
            this.Controls.Add(this.button_set2);
            this.Controls.Add(this.button_set1);
            this.Controls.Add(this.label_explain);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormSelectedArea";
            this.Text = "範囲選択（Escまたは閉じるボタンで終了）";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label_explain;
        private System.Windows.Forms.Button button_set1;
        private System.Windows.Forms.Button button_set2;
        private System.Windows.Forms.Button button_set3;
        private System.Windows.Forms.Button button_set4;
    }
}