﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace QuickQuips
{
    public partial class Form1 : Form
    {

        string savedQuips = Properties.Settings.Default.QuipFileLocation;
        KeyboardHook hook = new KeyboardHook();
        List<Quip> quips = null;
        bool displayed = false;
        public Form1()
        {
            InitializeComponent();
            hook.KeyPressed +=
           new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            hook.RegisterHotKey(QuickQuips.ModifierKeys.Shift | QuickQuips.ModifierKeys.Control, Keys.X);
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            if (File.Exists(savedQuips))
            {
                var stringOfQuips = File.ReadAllText(savedQuips);
                quips = JsonConvert.DeserializeObject<List<Quip>>(stringOfQuips);
            } else {
                quips = new List<Quip>();
            }
                 
            var buttons = createButtons(quips);
            foreach(var but in buttons)
            {
                flowLayoutPanel1.Controls.Add(but);
            }
        }

        public void reloadFlowLayout(List<Quip> list)
        {

            flowLayoutPanel1.Controls.Clear();
            var buttons = createButtons(quips);
            foreach (var but in buttons)
            {
                flowLayoutPanel1.Controls.Add(but);
            }
        }


        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // show the keys pressed in a label.
            
            if (!this.Visible)
            {
                this.TopMost = true;
                this.Show();

                    this.Location = new Point(Cursor.Position.X - this.Size.Width/2 , Cursor.Position.Y - this.Size.Height / 2);
                this.TopMost = false;
            }
            else
            {
                this.TopMost = false;
                this.Hide();
            }
            
            
        }

        private List<Button> createButtons(List<Quip> quips)
        {
            var buttons = new List<Button>();
            foreach (var item in quips)
            {
                var but = new Button();
                but.Click += item.executeQuip;
                but.Click += closeWindow;
                but.Size = new Size(100, 50);
                but.Text = item.ToString();
                buttons.Add(but);
            }
            return buttons;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void closeWindow(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var serialized = JsonConvert.SerializeObject(quips, Formatting.Indented);
            try
            {
                File.WriteAllText(savedQuips, serialized);
            } catch (Exception exe)
            {
                //shit
            }
        }

        private void goToJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(savedQuips);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new Settings();
            settingsForm.ShowDialog();
        }

        private void makeNewQuipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var manager = new QuipManager();
            manager.init(quips, this);
            manager.Show();
        }

        private void donateToCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }
    }
}
