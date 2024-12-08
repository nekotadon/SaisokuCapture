using System.Drawing;
using System.Windows.Forms;

namespace SaisokuCapture
{
    public partial class FormSelectedArea : Form
    {
        public FormSelectedArea(Icon icon)
        {
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

            //Escで終了
            KeyPreview = true;
            KeyDown += (sender, e) =>
            {
                if (e.KeyData == Keys.Escape)
                {
                    Close();
                }
            };

            //ラベル位置
            Load += (sender, e) => SetLabelPosition();
            SizeChanged += (sender, e) => SetLabelPosition();

            //透過色
            TransparencyKey = pictureBox.BackColor = Color.Red;

            //透過範囲の設定
            int left = button_set1.Left;
            int top = button_set1.Bottom + 5;
            int right = button_set4.Right;
            int bottom = button_set4.Top - 5;

            if (left > right)
            {
                right = left;
            }
            if (top > bottom)
            {
                bottom = top;
            }

            pictureBox.Width = right - left;
            pictureBox.Height = bottom - top;
            pictureBox.Location = new Point(left, top);
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            //フォームの位置とサイズ
            Load += (sender, e) =>
            {
                TextLib.IniFile iniFile = new TextLib.IniFile();
                int selectedAreaX = iniFile.GetKeyValueInt("capture", "SelectedAreaX", -1, true);
                int selectedAreaY = iniFile.GetKeyValueInt("capture", "SelectedAreaY", -1, true);
                int selectedAreaWidth = iniFile.GetKeyValueInt("capture", "SelectedAreaWidth", -1, true);
                int selectedAreaHeight = iniFile.GetKeyValueInt("capture", "SelectedAreaHeight", -1, true);
                if (selectedAreaHeight > 0 && selectedAreaWidth > 0)
                {
                    Width += selectedAreaWidth - pictureBox.Width;
                    Height += selectedAreaHeight - pictureBox.Height;
                    Point oldPoint = pictureBox.Parent.PointToScreen(pictureBox.Bounds.Location);
                    Point newPoint = new Point(selectedAreaX, selectedAreaY);
                    Location = new Point(Location.X + (newPoint.X - oldPoint.X), Location.Y + (newPoint.Y - oldPoint.Y));
                }
            };

            //マウスでコントロールを移動
            MouseDown += control_MouseDown;
            MouseUp += control_MouseUp;
            MouseMove += control_MouseMove;

        }

        private Point lastMousePosition;
        private bool mouseCapture;

        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastMousePosition = MousePosition;
                mouseCapture = true;
            }
        }

        private void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseCapture == false)
            {
                return;
            }

            //現在位置取得
            Point mp = MousePosition;

            //差分確認
            int offsetX = mp.X - lastMousePosition.X;
            int offsetY = mp.Y - lastMousePosition.Y;

            Location = new Point(Left + offsetX, Top + offsetY);

            lastMousePosition = mp;
        }

        private void control_MouseUp(object sender, MouseEventArgs e)
        {
            mouseCapture = false;
        }

        private void button_set_Click(object sender, System.EventArgs e)
        {
            Rectangle rect = pictureBox.Parent.RectangleToScreen(pictureBox.Bounds);

            TextLib.IniFile iniFile = new TextLib.IniFile();
            iniFile.SetKeyValueInt("capture", "SelectedAreaX", rect.X);
            iniFile.SetKeyValueInt("capture", "SelectedAreaY", rect.Y);
            iniFile.SetKeyValueInt("capture", "SelectedAreaWidth", rect.Width);
            iniFile.SetKeyValueInt("capture", "SelectedAreaHeight", rect.Height);
            Close();
        }

        private void SetLabelPosition()
        {
            label_explain.Left = (int)((ClientRectangle.Width - label_explain.Width) / 2.0);
        }
    }
}
