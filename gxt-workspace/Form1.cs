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
        List<KeyboardHook> kbhs;
        Movement mvt;
        public Form1()
        {
            InitializeComponent();
            mvt = new Movement(listBox1);
            kbhs = new List<KeyboardHook>();
            // mv left
            kbhs.Add(new KeyboardHook());
            kbhs.Last().KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            kbhs.Last().RegisterHotKey(gxt_workspace.ModifierKeys.Control | gxt_workspace.ModifierKeys.Alt,
                Keys.Left);

            // mv right
            kbhs.Add(new KeyboardHook());
            kbhs.Last().KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            kbhs.Last().RegisterHotKey(gxt_workspace.ModifierKeys.Control | gxt_workspace.ModifierKeys.Alt,
                Keys.Right);

            // mv up
            kbhs.Add(new KeyboardHook());
            kbhs.Last().KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            kbhs.Last().RegisterHotKey(gxt_workspace.ModifierKeys.Control | gxt_workspace.ModifierKeys.Alt,
                Keys.Up);

            // mv down
            kbhs.Add(new KeyboardHook());
            kbhs.Last().KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            kbhs.Last().RegisterHotKey(gxt_workspace.ModifierKeys.Control | gxt_workspace.ModifierKeys.Alt,
                Keys.Down);
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // show the keys pressed in a label.
            listBox1.Items.Add(e.Modifier.ToString() + " + " + e.Key.ToString());
            switch (e.Key)
            {
                case Keys.Left:
                    mvt.Move(Movement.MovementDirection.Left);
                    break;
                case Keys.Right:
                    mvt.Move(Movement.MovementDirection.Right);
                    break;
                case Keys.Up:
                    mvt.Move(Movement.MovementDirection.Up);
                    break;
                case Keys.Down:
                    mvt.Move(Movement.MovementDirection.Down);
                    break;
            }
        }
    }
}
