namespace SaisokuCapture
{
    partial class FormMain
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
            this.checkBox_setting_tray = new System.Windows.Forms.CheckBox();
            this.button_selectArea = new System.Windows.Forms.Button();
            this.checkBox_capture_SelectedArea = new System.Windows.Forms.CheckBox();
            this.checkBox_capture_ActiveWindow = new System.Windows.Forms.CheckBox();
            this.checkBox_capture_Screen = new System.Windows.Forms.CheckBox();
            this.button_end = new System.Windows.Forms.Button();
            this.label_explain = new System.Windows.Forms.Label();
            this.checkBox_save_jpg = new System.Windows.Forms.CheckBox();
            this.checkBox_save_png = new System.Windows.Forms.CheckBox();
            this.checkBox_save_bmp = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label_format = new System.Windows.Forms.Label();
            this.checkBox_capture_mouseCursor = new System.Windows.Forms.CheckBox();
            this.checkBox_setting_minboot = new System.Windows.Forms.CheckBox();
            this.checkBox_setting_topmost = new System.Windows.Forms.CheckBox();
            this.timerCapture = new System.Windows.Forms.Timer(this.components);
            this.button_openend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox_setting_tray
            // 
            this.checkBox_setting_tray.AutoSize = true;
            this.checkBox_setting_tray.Location = new System.Drawing.Point(25, 182);
            this.checkBox_setting_tray.Name = "checkBox_setting_tray";
            this.checkBox_setting_tray.Size = new System.Drawing.Size(165, 16);
            this.checkBox_setting_tray.TabIndex = 28;
            this.checkBox_setting_tray.Text = "最小化時にタスクトレイに配置";
            this.checkBox_setting_tray.UseVisualStyleBackColor = true;
            this.checkBox_setting_tray.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // button_selectArea
            // 
            this.button_selectArea.Location = new System.Drawing.Point(25, 101);
            this.button_selectArea.Name = "button_selectArea";
            this.button_selectArea.Size = new System.Drawing.Size(93, 23);
            this.button_selectArea.TabIndex = 27;
            this.button_selectArea.Text = "範囲設定";
            this.button_selectArea.UseVisualStyleBackColor = true;
            this.button_selectArea.Click += new System.EventHandler(this.button_selectArea_Click);
            // 
            // checkBox_capture_SelectedArea
            // 
            this.checkBox_capture_SelectedArea.AutoSize = true;
            this.checkBox_capture_SelectedArea.Location = new System.Drawing.Point(25, 79);
            this.checkBox_capture_SelectedArea.Name = "checkBox_capture_SelectedArea";
            this.checkBox_capture_SelectedArea.Size = new System.Drawing.Size(72, 16);
            this.checkBox_capture_SelectedArea.TabIndex = 26;
            this.checkBox_capture_SelectedArea.Text = "指定範囲";
            this.checkBox_capture_SelectedArea.UseVisualStyleBackColor = true;
            this.checkBox_capture_SelectedArea.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox_capture_ActiveWindow
            // 
            this.checkBox_capture_ActiveWindow.AutoSize = true;
            this.checkBox_capture_ActiveWindow.Location = new System.Drawing.Point(122, 57);
            this.checkBox_capture_ActiveWindow.Name = "checkBox_capture_ActiveWindow";
            this.checkBox_capture_ActiveWindow.Size = new System.Drawing.Size(109, 16);
            this.checkBox_capture_ActiveWindow.TabIndex = 25;
            this.checkBox_capture_ActiveWindow.Text = "アクティブウィンドウ";
            this.checkBox_capture_ActiveWindow.UseVisualStyleBackColor = true;
            this.checkBox_capture_ActiveWindow.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox_capture_Screen
            // 
            this.checkBox_capture_Screen.AutoSize = true;
            this.checkBox_capture_Screen.Location = new System.Drawing.Point(25, 57);
            this.checkBox_capture_Screen.Name = "checkBox_capture_Screen";
            this.checkBox_capture_Screen.Size = new System.Drawing.Size(91, 16);
            this.checkBox_capture_Screen.TabIndex = 24;
            this.checkBox_capture_Screen.Text = "スクリーン全体";
            this.checkBox_capture_Screen.UseVisualStyleBackColor = true;
            this.checkBox_capture_Screen.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // button_end
            // 
            this.button_end.Location = new System.Drawing.Point(10, 256);
            this.button_end.Name = "button_end";
            this.button_end.Size = new System.Drawing.Size(75, 23);
            this.button_end.TabIndex = 23;
            this.button_end.Text = "終了";
            this.button_end.UseVisualStyleBackColor = true;
            this.button_end.Click += new System.EventHandler(this.button_end_Click);
            // 
            // label_explain
            // 
            this.label_explain.AutoSize = true;
            this.label_explain.Location = new System.Drawing.Point(12, 9);
            this.label_explain.Name = "label_explain";
            this.label_explain.Size = new System.Drawing.Size(188, 12);
            this.label_explain.TabIndex = 21;
            this.label_explain.Text = "ソフト起動中は1秒ごとにキャプチャ実行";
            // 
            // checkBox_save_jpg
            // 
            this.checkBox_save_jpg.AutoSize = true;
            this.checkBox_save_jpg.Location = new System.Drawing.Point(181, 227);
            this.checkBox_save_jpg.Name = "checkBox_save_jpg";
            this.checkBox_save_jpg.Size = new System.Drawing.Size(39, 16);
            this.checkBox_save_jpg.TabIndex = 20;
            this.checkBox_save_jpg.Text = "jpg";
            this.checkBox_save_jpg.UseVisualStyleBackColor = true;
            this.checkBox_save_jpg.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox_save_png
            // 
            this.checkBox_save_png.AutoSize = true;
            this.checkBox_save_png.Location = new System.Drawing.Point(133, 227);
            this.checkBox_save_png.Name = "checkBox_save_png";
            this.checkBox_save_png.Size = new System.Drawing.Size(42, 16);
            this.checkBox_save_png.TabIndex = 19;
            this.checkBox_save_png.Text = "png";
            this.checkBox_save_png.UseVisualStyleBackColor = true;
            this.checkBox_save_png.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox_save_bmp
            // 
            this.checkBox_save_bmp.AutoSize = true;
            this.checkBox_save_bmp.Location = new System.Drawing.Point(82, 227);
            this.checkBox_save_bmp.Name = "checkBox_save_bmp";
            this.checkBox_save_bmp.Size = new System.Drawing.Size(45, 16);
            this.checkBox_save_bmp.TabIndex = 18;
            this.checkBox_save_bmp.Text = "bmp";
            this.checkBox_save_bmp.UseVisualStyleBackColor = true;
            this.checkBox_save_bmp.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "キャプチャ対象";
            // 
            // label_format
            // 
            this.label_format.AutoSize = true;
            this.label_format.Location = new System.Drawing.Point(23, 228);
            this.label_format.Name = "label_format";
            this.label_format.Size = new System.Drawing.Size(53, 12);
            this.label_format.TabIndex = 15;
            this.label_format.Text = "保存形式";
            // 
            // checkBox_capture_mouseCursor
            // 
            this.checkBox_capture_mouseCursor.AutoSize = true;
            this.checkBox_capture_mouseCursor.Location = new System.Drawing.Point(25, 204);
            this.checkBox_capture_mouseCursor.Name = "checkBox_capture_mouseCursor";
            this.checkBox_capture_mouseCursor.Size = new System.Drawing.Size(161, 16);
            this.checkBox_capture_mouseCursor.TabIndex = 14;
            this.checkBox_capture_mouseCursor.Text = "マウスカーソルをキャプチャする";
            this.checkBox_capture_mouseCursor.UseVisualStyleBackColor = true;
            this.checkBox_capture_mouseCursor.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox_setting_minboot
            // 
            this.checkBox_setting_minboot.AutoSize = true;
            this.checkBox_setting_minboot.Location = new System.Drawing.Point(122, 160);
            this.checkBox_setting_minboot.Name = "checkBox_setting_minboot";
            this.checkBox_setting_minboot.Size = new System.Drawing.Size(105, 16);
            this.checkBox_setting_minboot.TabIndex = 13;
            this.checkBox_setting_minboot.Text = "起動時に最小化";
            this.checkBox_setting_minboot.UseVisualStyleBackColor = true;
            this.checkBox_setting_minboot.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox_setting_topmost
            // 
            this.checkBox_setting_topmost.AutoSize = true;
            this.checkBox_setting_topmost.Location = new System.Drawing.Point(25, 160);
            this.checkBox_setting_topmost.Name = "checkBox_setting_topmost";
            this.checkBox_setting_topmost.Size = new System.Drawing.Size(84, 16);
            this.checkBox_setting_topmost.TabIndex = 12;
            this.checkBox_setting_topmost.Text = "最前面表示";
            this.checkBox_setting_topmost.UseVisualStyleBackColor = true;
            this.checkBox_setting_topmost.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // button_openend
            // 
            this.button_openend.Location = new System.Drawing.Point(91, 256);
            this.button_openend.Name = "button_openend";
            this.button_openend.Size = new System.Drawing.Size(154, 23);
            this.button_openend.TabIndex = 22;
            this.button_openend.Text = "保存先を開いて終了";
            this.button_openend.UseVisualStyleBackColor = true;
            this.button_openend.Click += new System.EventHandler(this.button_openend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "その他";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 288);
            this.Controls.Add(this.checkBox_setting_tray);
            this.Controls.Add(this.button_selectArea);
            this.Controls.Add(this.checkBox_capture_SelectedArea);
            this.Controls.Add(this.checkBox_capture_ActiveWindow);
            this.Controls.Add(this.checkBox_capture_Screen);
            this.Controls.Add(this.button_end);
            this.Controls.Add(this.label_explain);
            this.Controls.Add(this.checkBox_save_jpg);
            this.Controls.Add(this.checkBox_save_png);
            this.Controls.Add(this.checkBox_save_bmp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_format);
            this.Controls.Add(this.checkBox_capture_mouseCursor);
            this.Controls.Add(this.checkBox_setting_minboot);
            this.Controls.Add(this.checkBox_setting_topmost);
            this.Controls.Add(this.button_openend);
            this.Controls.Add(this.label1);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_setting_tray;
        private System.Windows.Forms.Button button_selectArea;
        private System.Windows.Forms.CheckBox checkBox_capture_SelectedArea;
        private System.Windows.Forms.CheckBox checkBox_capture_ActiveWindow;
        private System.Windows.Forms.CheckBox checkBox_capture_Screen;
        private System.Windows.Forms.Button button_end;
        private System.Windows.Forms.Label label_explain;
        private System.Windows.Forms.CheckBox checkBox_save_jpg;
        private System.Windows.Forms.CheckBox checkBox_save_png;
        private System.Windows.Forms.CheckBox checkBox_save_bmp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_format;
        private System.Windows.Forms.CheckBox checkBox_capture_mouseCursor;
        private System.Windows.Forms.CheckBox checkBox_setting_minboot;
        private System.Windows.Forms.CheckBox checkBox_setting_topmost;
        private System.Windows.Forms.Timer timerCapture;
        private System.Windows.Forms.Button button_openend;
        private System.Windows.Forms.Label label1;
    }
}