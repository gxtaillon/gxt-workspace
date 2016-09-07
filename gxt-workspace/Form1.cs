using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gxt_workspace
{
    public partial class Form1 : Form
    {
        KeyboardHook kbh;
        Movement mvt;
        public Form1()
        {
            InitializeComponent();
            kbh = new KeyboardHook();
            mvt = new Movement();
            mvt.Test();
            // register the event that is fired after the key press.
            kbh.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            kbh.RegisterHotKey(gxt_workspace.ModifierKeys.Control | gxt_workspace.ModifierKeys.Alt,
                Keys.Left);
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // show the keys pressed in a label.
            MessageBox.Show(e.Modifier.ToString() + " + " + e.Key.ToString());
        }

    }
}
