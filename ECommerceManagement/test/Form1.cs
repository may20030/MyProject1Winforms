using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ECommerceManagement.Application.Interfaces.Services;

namespace test
{
    public class LoginForm : Form
    {
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;

        private IUserService _userService;

        public LoginForm(IUserService userService)
        {
            _userService = userService;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Form
            this.Text = "Login";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Username Label
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new Point(50, 50);
            lblUsername.Size = new Size(80, 25);

            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Location = new Point(150, 50);
            txtUsername.Size = new Size(180, 25);

            // Password Label
            lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new Point(50, 100);
            lblPassword.Size = new Size(80, 25);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Location = new Point(150, 100);
            txtPassword.Size = new Size(180, 25);
            txtPassword.UseSystemPasswordChar = true;

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new Point(150, 150);
            btnLogin.Size = new Size(100, 30);
            btnLogin.Click += BtnLogin_Click;

            // Add controls to form
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // مثال: لو عندك Autofac container
            var userDto = await _userService.LoginAsync(new ECommerceManagement.Application.DTOs.UserDTOs.LoginDTO
            {
                Username = txtUsername.Text,
                Password = txtPassword.Text
            });

            if (userDto != null)
            {
                MessageBox.Show($"Welcome {userDto.Username}!");
            }
            else
            {
                MessageBox.Show("Invalid credentials, try again.");
            }
        }
    }
}
