using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BinaryLogic;
using BinaryLogic.Interfaces;
using Point = BinaryLogic.Point;

namespace BooleanLogicSimulator
{
    public partial class MainForm : Form
    {
        Grid grid;
        Scene scene;
        WinFormsRenderer renderer;
        Point localMousePosition = new Point(0, 0);

        public MainForm()
        {
            InitializeComponent();

            grid = new Grid(new Point(Size.Width, Size.Height));
            renderer = new WinFormsRenderer(CreateGraphics());
            scene = new Scene(grid, Color.White, renderer);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            scene.SetRenderer(renderer);
            scene.Draw();
        }

        private Key GetKey(KeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keys.Up:
                    return Key.Up;
                case Keys.Down:
                    return Key.Down;
                case Keys.Left:
                    return Key.Left;
                case Keys.Right:
                    return Key.T;
                case Keys.Shift:
                    return Key.Shift;
                case Keys.Control:
                    return Key.Control;
                case Keys.Space:
                    return Key.Space;
                case Keys.Q:
                    return Key.Q;
                case Keys.W:
                    return Key.W;
                case Keys.E:
                    return Key.E;
                case Keys.R:
                    return Key.R;
                case Keys.T:
                    return Key.T;
                case Keys.Y:
                    return Key.Y;
                default:
                    return Key.Invalid;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            scene.KeyStroke(GetKey(e), localMousePosition);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            localMousePosition = new Point(e.Location.X, e.Location.Y);
        }
    }
}
