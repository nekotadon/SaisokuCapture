using System;
using System.Drawing;
using System.Windows.Forms;

namespace SaisokuCapture
{
    public partial class Form1 : Form
    {
        FormMain formMain = null;
        TextLib.IniFile iniFile = null;

        public Form1()
        {
            InitializeComponent();

            //ソフトウェア名
            string AppName = "最速キャプチャ";

            //トレイアイコン
            notifyIcon_tray.Icon = Icon;
            notifyIcon_tray.Text = AppName;
            notifyIcon_tray.Visible = false;

            //メインフォーム
            formMain = new FormMain(Handle, AppName, Icon);

            //設定ファイル
            iniFile = new TextLib.IniFile();

            bool minBoot = iniFile.GetKeyValueBool("setting", "minboot", false, true);//最小化起動
            bool tray = iniFile.GetKeyValueBool("setting", "tray", false, true);//トレイに収納

            //起動位置の設定
            SetBootPosition();

            //起動
            if (minBoot)
            {
                if (tray)
                {
                    //トレイアイコンのみ
                    notifyIcon_tray.Visible = true;
                }
                else
                {
                    //通常起動＆最小化
                    formMain.WindowState = FormWindowState.Minimized;
                    formMain.Show();
                }
            }
            else
            {
                //通常起動
                formMain.StartPosition = FormStartPosition.Manual;
                formMain.Show();
            }
        }

        //ウィンドウプロシージャ
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //メインフォームが最小化したとき
                case Win32API.WM_APP + 1:
                    {
                        //トレイに最小化する設定であれば
                        bool tray = iniFile.GetKeyValueBool("setting", "tray", false, true);
                        if (tray)
                        {
                            //トレイアイコン表示
                            notifyIcon_tray.Visible = true;

                            //メインフォーム非表示
                            formMain.Visible = false;
                        }
                    }
                    break;
            }

            //通常のウィンドウプロシージャ処理
            base.WndProc(ref m);
        }

        //起動位置の設定
        void SetBootPosition()
        {
            Rectangle screen = Screen.GetBounds(this);
            int centerTop = screen.Y + screen.Height / 2 - Height / 2;
            int centerLeft = screen.X + screen.Width / 2 - Width / 2;

            int top = iniFile.GetKeyValueInt("pos", "Top", centerTop, true);
            int left = iniFile.GetKeyValueInt("pos", "Left", centerLeft, true);

            if (screen.X <= left && left <= screen.X + screen.Width && screen.Y <= top && top <= screen.Y + screen.Height)
            {
                formMain.Top = top;
                formMain.Left = left;
            }
            else
            {
                formMain.Top = centerTop;
                formMain.Left = centerLeft;
            }
        }

        //トレイアイコン

        //メニュー選択orマウス左クリックで元のサイズに戻す
        private void notifyIcon_tray_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FormMainShow();
            }
        }
        private void 元のサイズに戻すToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMainShow();
        }
        void FormMainShow()
        {
            //トレイアイコン非表示
            notifyIcon_tray.Visible = false;

            //フォーム表示
            formMain.Visible = true;
            formMain.WindowState = FormWindowState.Normal;
            SetBootPosition();
        }

        //終了
        private void 保存先を開いて終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon_tray.Visible = false;
            formMain.AppClose(true);
        }
        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon_tray.Visible = false;
            formMain.AppClose();
        }
    }

    //win32API定数
    internal static class Win32API
    {
        public const int WM_APP = 0x0800;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MINIMIZE = 0xF020;
    }
}
