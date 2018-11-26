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

namespace BooleanLogicSimulator
{
    public partial class MainForm : Form, IGraphics
    {
        public MainForm()
        {
            InitializeComponent();
        }
    }
}
