using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SaisokuCapture
{
    public partial class FormMain : Form
    {
        //---------- Win32API用定義開始 ----------

        //Windows APIの定義
        internal static class NativeMethods
        {
            //メッセージの送信
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PostMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

            //マウスカーソルの取得
            [DllImport("user32.dll")]
            public static extern bool GetCursorInfo(ref CURSORINFO pci);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public uint cbSize;
            public uint flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        //---------- Win32API用定義終了 ----------

        IntPtr AppHandle = IntPtr.Zero;
        public string imagefolder = "";
        bool processing = false;
        TextLib.IniFile iniFile = null;
        Rectangle? selectedArea = null;

        public FormMain(IntPtr handle, string name, Icon icon)
        {
            AppHandle = handle;
            InitializeComponent();

            //アイコン
            if (icon != null)
            {
                Icon = icon;
            }

            //フォント
            foreach (Control control in Controls)
            {
                if (control.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)control).Font = SystemInformation.MenuFont;
                }
                else if (control.GetType() == typeof(Label))
                {
                    ((Label)control).Font = SystemInformation.MenuFont;
                }
                else if (control.GetType() == typeof(Button))
                {
                    ((Button)control).Font = SystemInformation.MenuFont;
                }
            }

            //ウィンドウ
            MaximumSize = MinimumSize = Size;//サイズ固定
            MaximizeBox = false;//最大化ボタン無効

            //タイトル
            Text = name;

            //保存先フォルダパス
            string filepath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(filepath);
            imagefolder = directory + @"\captureImage\";

            ///////////////////////////
            //  設定ファイル読み込み
            ///////////////////////////

            //設定ファイル
            iniFile = new TextLib.IniFile();
            Rectangle screen = Screen.GetBounds(this);

            //スクリーン、ウィンドウキャプチャ
            checkBox_capture_Screen.Checked = iniFile.GetKeyValueBool("capture", "Screen", true, true);
            checkBox_capture_ActiveWindow.Checked = iniFile.GetKeyValueBool("capture", "ActiveWindow", true, true);

            //指定範囲キャプチャ
            checkBox_capture_SelectedArea.Checked = iniFile.GetKeyValueBool("capture", "SelectedArea", false, true);
            SetSelectedArea();

            //最小化起動、最前面表示、タスクトレイに収納
            checkBox_setting_minboot.Checked = iniFile.GetKeyValueBool("setting", "minboot", false, true);
            checkBox_setting_topmost.Checked = TopMost = iniFile.GetKeyValueBool("setting", "topmost", false, true);
            checkBox_setting_tray.Checked = iniFile.GetKeyValueBool("setting", "tray", false, true);

            //保存形式
            checkBox_save_bmp.Checked = iniFile.GetKeyValueBool("save", "bmp", false, true);
            checkBox_save_png.Checked = iniFile.GetKeyValueBool("save", "png", true, true);
            checkBox_save_jpg.Checked = iniFile.GetKeyValueBool("save", "jpg", false, true);

            //保存間隔msec
            int interval = iniFile.GetKeyValueInt("save", "msec", 1000, true);

            //マウスカーソルキャプチャ
            checkBox_capture_mouseCursor.Checked = iniFile.GetKeyValueBool("capture", "mouseCursor", true, true);

            //保存間隔

            if (interval <= 0)
            {
                interval = 1;
            }
            timerCapture.Interval = interval;

            if (interval < 1000)//1秒未満
            {
                label_explain.Text = "ソフト起動中は" + interval.ToString() + "ミリ秒ごとにキャプチャ実行";
            }
            else//1秒以上
            {
                if (interval % 1000 == 0)//秒単位
                {
                    label_explain.Text = "ソフト起動中は" + (interval / 1000).ToString() + "秒ごとにキャプチャ実行";
                }
                else
                {
                    label_explain.Text = "ソフト起動中は定期的にキャプチャ実行";
                }
            }

            ///////////////////////////
            //  指定間隔でキャプチャ
            ///////////////////////////

            //画像保存先フォルダの作成
            MakeDirectory();

            //指定間隔でキャプチャ
            timerCapture.Tick += (sender, e) =>
            {
                if (!processing)
                {
                    processing = true;

                    //スクリーンキャプチャ
                    if (checkBox_capture_Screen.Checked)
                    {
                        //各スクリーンキャプチャ
                        foreach (var targetScreen in Screen.AllScreens)
                        {
                            CaptureRectangle(targetScreen.Bounds, "Screen" + Regex.Replace(targetScreen.DeviceName, "\\W", "") + "_");
                        }

                        //仮想画面キャプチャ
                        if (Screen.AllScreens.Length >= 2)//スクリーンが2個以上ある場合
                        {
                            CaptureRectangle(GetVirtualScreen(), "VirtualScreen_");
                        }
                    }

                    //アクティブウィンドウキャプチャ
                    if (checkBox_capture_ActiveWindow.Checked)
                    {
                        //ウィンドウ可視領域
                        CaptureRectangle(WindowSizeMethod.GetActiveWindowVisibleArea() ?? new Rectangle(0, 0, 0, 0), "ActiveWindowVisible_");

                        //ウィンドウ領域
                        CaptureRectangle(WindowSizeMethod.GetActiveWindowArea() ?? new Rectangle(0, 0, 0, 0), "ActiveWindow_");
                    }

                    //指定範囲キャプチャ
                    if (checkBox_capture_SelectedArea.Checked && selectedArea != null)
                    {
                        CaptureRectangle(selectedArea ?? new Rectangle(0, 0, 0, 0), "SelectedArea_");
                    }

                    processing = false;
                }
            };

            //起動と同時にキャプチャ
            timerCapture.Enabled = true;

            //終了
            FormClosing += (sender, e) => AppClose();
        }

        /////////////////////////
        //  ウィンドウプロシージャ
        /////////////////////////
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32API.WM_SYSCOMMAND:
                    if (((int)m.WParam & 0xFFFF) == Win32API.SC_MINIMIZE)//最小化するとき
                    {
                        //最小化する前に位置を保存
                        SavePosition();

                        //最小化することを連絡
                        if (AppHandle != IntPtr.Zero)
                        {
                            NativeMethods.PostMessage(AppHandle, Win32API.WM_APP + 1, UIntPtr.Zero, IntPtr.Zero);
                        }
                    }
                    break;
            }

            //通常のウィンドウプロシージャ処理
            base.WndProc(ref m);
        }

        /////////////////////////
        //  コントロール操作時
        /////////////////////////

        //checkbox
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string[] name = checkBox.Name.Split('_');//checkBox_setting_minboot -> checkBox setting minboot
            if (name.Length == 3)
            {
                iniFile.SetKeyValueBool(name[1], name[2], checkBox.Checked);
            }
            TopMost = checkBox_setting_topmost.Checked;
        }

        //閉じるボタン
        private void button_end_Click(object sender, EventArgs e)
        {
            AppClose();
        }
        private void button_openend_Click(object sender, EventArgs e)
        {
            AppClose(true);
        }

        //終了処理
        public void AppClose(bool open = false)
        {
            //キャプチャ終了
            timerCapture.Enabled = false;

            //終了時の位置を保存
            SavePosition();

            //フォルダを開く
            if (open)
            {
                if (Directory.Exists(imagefolder))
                {
                    Process.Start(imagefolder);
                }
            }

            //終了
            Application.Exit();
        }

        //終了時の位置を保存
        void SavePosition()
        {
            if (Visible && WindowState == FormWindowState.Normal && GetVirtualScreen().Contains(Location))
            {
                iniFile.SetKeyValueInt("pos", "Top", Top);
                iniFile.SetKeyValueInt("pos", "Left", Left);
            }
        }

        //指定範囲キャプチャ領域の設定ボタン
        private void button_selectArea_Click(object sender, EventArgs e)
        {
            FormSelectedArea formSelectedArea = new FormSelectedArea(Icon);
            formSelectedArea.ShowDialog(this);
            formSelectedArea.Dispose();

            SetSelectedArea();
        }

        //指定領域の設定
        private void SetSelectedArea()
        {
            //設定読み込み
            int selectedAreaX = iniFile.GetKeyValueInt("capture", "SelectedAreaX", -1, true);
            int selectedAreaY = iniFile.GetKeyValueInt("capture", "SelectedAreaY", -1, true);
            int selectedAreaWidth = iniFile.GetKeyValueInt("capture", "SelectedAreaWidth", -1, true);
            int selectedAreaHeight = iniFile.GetKeyValueInt("capture", "SelectedAreaHeight", -1, true);

            //設定の正しさ確認して設定修正保存
            bool correct = false;
            if (selectedAreaHeight > 0 && selectedAreaWidth > 0)
            {
                Rectangle rawRect = new Rectangle(selectedAreaX, selectedAreaY, selectedAreaWidth, selectedAreaHeight);
                Rectangle? visibleRect = GetVisibleRect(rawRect);
                if (visibleRect != null)
                {
                    //修正領域を設定
                    correct = true;
                    Rectangle rect = visibleRect ?? new Rectangle(0, 0, 0, 0);
                    iniFile.SetKeyValueInt("capture", "SelectedAreaX", rect.X);
                    iniFile.SetKeyValueInt("capture", "SelectedAreaY", rect.Y);
                    iniFile.SetKeyValueInt("capture", "SelectedAreaWidth", rect.Width);
                    iniFile.SetKeyValueInt("capture", "SelectedAreaHeight", rect.Height);
                }
            }
            if (!correct)
            {
                //初期化
                iniFile.SetKeyValueInt("capture", "SelectedAreaX", -1);
                iniFile.SetKeyValueInt("capture", "SelectedAreaY", -1);
                iniFile.SetKeyValueInt("capture", "SelectedAreaWidth", -1);
                iniFile.SetKeyValueInt("capture", "SelectedAreaHeight", -1);
            }

            //再読み込み
            selectedAreaX = iniFile.GetKeyValueInt("capture", "SelectedAreaX", -1, true);
            selectedAreaY = iniFile.GetKeyValueInt("capture", "SelectedAreaY", -1, true);
            selectedAreaWidth = iniFile.GetKeyValueInt("capture", "SelectedAreaWidth", -1, true);
            selectedAreaHeight = iniFile.GetKeyValueInt("capture", "SelectedAreaHeight", -1, true);

            //指定範囲領域の設定
            if (selectedAreaHeight > 0 && selectedAreaWidth > 0)
            {
                selectedArea = new Rectangle(selectedAreaX, selectedAreaY, selectedAreaWidth, selectedAreaHeight);
                checkBox_capture_SelectedArea.Text = $"指定範囲(x{selectedAreaX},y{selectedAreaY},w{selectedAreaWidth},h{selectedAreaHeight})";
            }
            else
            {
                selectedArea = null;
                checkBox_capture_SelectedArea.Text = "指定範囲(範囲指定なし)";
            }
        }

        //////////////////////
        //  キャプチャ処理
        //////////////////////

        //画像保存先の作成
        private bool MakeDirectory()
        {
            if (Directory.Exists(imagefolder))
            {
                return true;
            }
            else
            {
                try
                {
                    DirectoryInfo di = Directory.CreateDirectory(imagefolder);
                    return true;
                }
                catch (Exception)
                {
                    ;
                }
            }

            return false;
        }

        //領域キャプチャと保存
        private void CaptureRectangle(Rectangle rectangle, string name)
        {
            if (rectangle.Width != 0 && rectangle.Height != 0)
            {
                using (Bitmap bmp = new Bitmap(rectangle.Width, rectangle.Height))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        try
                        {
                            //領域キャプチャ
                            g.CopyFromScreen(rectangle.Location, new Point(0, 0), bmp.Size);

                            //マウスカーソル取得
                            if (checkBox_capture_mouseCursor.Checked)
                            {
                                CURSORINFO cursorinfo = new CURSORINFO();
                                cursorinfo.cbSize = (uint)Marshal.SizeOf(cursorinfo);
                                if (NativeMethods.GetCursorInfo(ref cursorinfo))
                                {
                                    Cursor cursor = new Cursor(cursorinfo.hCursor);
                                    Point hotSpot = cursor.HotSpot;
                                    Point cursorScreenPosition = Cursor.Position;
                                    g.DrawIcon(Icon.FromHandle(cursorinfo.hCursor), cursorScreenPosition.X - hotSpot.X - rectangle.X, cursorScreenPosition.Y - hotSpot.Y - rectangle.Y);
                                }
                            }

                            if (MakeDirectory())
                            {
                                string filename = imagefolder + name + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                                if (checkBox_save_bmp.Checked)
                                {
                                    bmp.Save(filename + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                                if (checkBox_save_png.Checked)
                                {
                                    bmp.Save(filename + ".png", System.Drawing.Imaging.ImageFormat.Png);
                                }
                                if (checkBox_save_jpg.Checked)
                                {
                                    bmp.Save(filename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ;
                        }
                    }
                }
            }
        }

        //ウィンドウ領域の取得
        public static class WindowSizeMethod
        {
            private const uint DWMWA_EXTENDED_FRAME_BOUNDS = 9;

            private struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            private static class NativeMethods
            {
                [DllImport("dwmapi.dll")]
                internal static extern int DwmGetWindowAttribute(IntPtr hWnd, uint dwAttribute, out RECT pvAttribute, int cbAttribute);

                [DllImport("user32.dll")]
                internal static extern int GetWindowRect(IntPtr hWnd, out RECT lpRect);

                [DllImport("user32.dll")]
                internal static extern IntPtr GetForegroundWindow();
            }

            public static Rectangle? GetActiveWindowVisibleArea()
            {
                IntPtr hWnd = NativeMethods.GetForegroundWindow();
                if (hWnd != IntPtr.Zero)
                {
                    RECT rect;
                    int ret = NativeMethods.DwmGetWindowAttribute(hWnd, DWMWA_EXTENDED_FRAME_BOUNDS, out rect, Marshal.SizeOf(typeof(RECT)));
                    if (ret == 0)
                    {
                        return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
                    }
                }

                return null;
            }

            public static Rectangle? GetActiveWindowArea()
            {
                IntPtr hWnd = NativeMethods.GetForegroundWindow();
                if (hWnd != IntPtr.Zero)
                {
                    RECT rect;
                    int ret = NativeMethods.GetWindowRect(hWnd, out rect);
                    if (ret != 0)
                    {
                        return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
                    }
                }

                return null;
            }
        }

        //仮想画面の取得
        private Rectangle GetVirtualScreen()
        {
            return GetCombinedScreen(Screen.AllScreens);
        }

        //結合スクリーン領域の取得
        private Rectangle GetCombinedScreen(Screen[] screens)
        {
            int top = 0, left = 0, bottom = 0, right = 0;
            bool first = true;
            foreach (var screen in screens)
            {
                if (first)
                {
                    first = false;
                    top = screen.Bounds.Top;
                    left = screen.Bounds.Left;
                    bottom = screen.Bounds.Bottom;
                    right = screen.Bounds.Right;
                }

                if (screen.Bounds.Top < top)
                {
                    top = screen.Bounds.Top;
                }
                if (screen.Bounds.Left < left)
                {
                    left = screen.Bounds.Left;
                }
                if (bottom < screen.Bounds.Bottom)
                {
                    bottom = screen.Bounds.Bottom;
                }
                if (right < screen.Bounds.Right)
                {
                    right = screen.Bounds.Right;
                }
            }

            return new Rectangle(left, top, right - left, bottom - top);
        }

        //領域を含むスクリーンの取得
        private Screen[] GetScreensFromRect(Rectangle rect)
        {
            List<Screen> screens = new List<Screen>();

            foreach (var screen in Screen.AllScreens)
            {
                if (rect.Right < screen.Bounds.Left//スクリーンの左側の場合
                || screen.Bounds.Right < rect.Left//スクリーンの右側の場合
                || rect.Bottom < screen.Bounds.Top //スクリーンの上側の場合
                || screen.Bounds.Bottom < rect.Top)//スクリーンの下側の場合
                {
                    ;//何もしない
                }
                else
                {
                    screens.Add(screen);
                }
            }

            return screens.ToArray();
        }

        //指定領域のスクリーン内領域の取得
        private Rectangle? GetVisibleRect(Rectangle rect)
        {
            //指定領域を含むスクリーン
            Screen[] screens = GetScreensFromRect(rect);

            //指定領域を含むスクリーンを結合した領域
            Rectangle combinedScreen = GetCombinedScreen(screens);

            if (combinedScreen.Right < rect.Left || combinedScreen.Bottom < rect.Top)//領域の左上の点が範囲外の場合
            {
                return null;
            }
            else if (rect.Bottom < combinedScreen.Top || rect.Right < combinedScreen.Left)//領域の右下の点が範囲外の場合
            {
                return null;
            }
            else
            {
                //Left
                int left = combinedScreen.Left;
                if (left < rect.Left)
                {
                    left = rect.Left;
                }

                //Top
                int top = combinedScreen.Top;
                if (top < rect.Top)
                {
                    top = rect.Top;
                }

                //Right
                int right = combinedScreen.Right;
                if (rect.Right < right)
                {
                    right = rect.Right;
                }

                //Bottom
                int bottom = combinedScreen.Bottom;
                if (rect.Bottom < bottom)
                {
                    bottom = rect.Bottom;
                }

                return new Rectangle(left, top, right - left, bottom - top);
            }
        }
    }
}
