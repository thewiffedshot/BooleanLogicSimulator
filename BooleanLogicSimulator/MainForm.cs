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

            renderer = new WinFormsRenderer(e.Graphics);
            scene.SetRenderer(renderer);
            scene.Draw();
        }
    }
}
