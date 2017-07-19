﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrappyListenMoe
{
	public partial class FormLogin : Form
	{
		Action<bool, string, string, string> tokenCallback;

		//Empty constructor only so the designer can be used. Do not put anything in here, it won't be called.
		public FormLogin()
		{
			InitializeComponent();
		}

		public FormLogin(Action<bool, string, string, string> tokenCallback)
		{
			InitializeComponent();
			button1.Font = OpenSans.GetFont(11);
			textBox1.Font = OpenSans.GetFont(9);
			textBox2.Font = OpenSans.GetFont(9);
            checkBox1.Font = OpenSans.GetFont(9);
            checkBox2.Font = OpenSans.GetFont(9);
            checkBox3.Font = OpenSans.GetFont(9);
            label1.Font = OpenSans.GetFont(8);

            checkBox1.Checked = Settings.GetBoolSetting("TopMost");
            checkBox2.Checked = Settings.GetBoolSetting("IgnoreUpdates");
            checkBox3.Checked = Settings.GetBoolSetting("CloseToTray");

            var username = Settings.GetStringSetting("Username").Trim();

			if (username != "")
			{
				var loginString = "Logged in as " + username;
                label1.Text = loginString;
			}

			this.tokenCallback = tokenCallback;
		}

		private async Task Login()
		{
			var postData = new Dictionary<string, string>();
			postData.Add("username", textBox1.Text);
			postData.Add("password", textBox2.Text);

			string resp = await WebHelper.Post("https://listen.moe/api/authenticate", postData);
			var response = Json.Parse<AuthenticateResponse>(resp);
			tokenCallback(response.success, response.token, textBox1.Text, response.message ?? "");
			this.Close();
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			await Login();
		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SetBoolSetting("TopMost", checkBox1.Checked);
            Settings.WriteSettings();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SetBoolSetting("IgnoreUpdates", checkBox2.Checked);
            Settings.WriteSettings();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.SetBoolSetting("CloseToTray", checkBox3.Checked);
            Settings.WriteSettings();
        }
    }
}
