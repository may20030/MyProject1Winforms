using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Application.DTOs.UserDTOs;
using System.Threading.Tasks;

namespace ECommerceManagement.Presentation.Forms.LoginForms
{
    public class SignUpForm : Form
    {
        private readonly IUserService _userService;
        private WebView2 webView;

        // Notification Panel
        private Panel notificationPanel;
        private Label notificationLabel;

        public SignUpForm(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();
            InitializeNotificationPanel();
        }

        private async void InitializeComponent()
        {
            this.Text = "Sign Up";
            this.Width = 800;
            this.Height = 800;

            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            this.Controls.Add(webView);

            this.Load += SignUpForm_Load;
        }

        private async void SignUpForm_Load(object sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async();

            string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "UI", "signup.html");
            webView.Source = new Uri(path);

            webView.CoreWebView2.WebMessageReceived += WebMessageReceived;
        }

        // =========================
        // Notification Panel
        // =========================
        private void InitializeNotificationPanel()
        {
            notificationPanel = new Panel
            {
                Width = 400,
                Height = 100,
                BorderStyle = BorderStyle.None,
                Visible = false
            };

            notificationLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold)
            };

            notificationPanel.Controls.Add(notificationLabel);

            // مركزية
            notificationPanel.Left = (this.ClientSize.Width - notificationPanel.Width) / 2;
            notificationPanel.Top = 50; // يظهر فوق الصفحة بدل ما يكون في النص


            this.Controls.Add(notificationPanel);
        }

        private async Task ShowNotificationAsync(string message, bool isSuccess)
        {
            if (this.InvokeRequired)
            {
                await this.Invoke(new Func<Task>(() => ShowNotificationAsync(message, isSuccess)));
                return;
            }

            notificationLabel.Text = message;
            notificationPanel.BackColor = isSuccess
                ? System.Drawing.Color.FromArgb(76, 175, 80)
                : System.Drawing.Color.FromArgb(244, 67, 54);

            notificationPanel.BringToFront();
            notificationPanel.Visible = true;

            await Task.Delay(2500); // مدة عرض الرسالة
            notificationPanel.Visible = false;
        }

        // =========================
        // WebView2 Message Handler
        // =========================
        private async void WebMessageReceived(object sender,
            Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(e.WebMessageAsJson);
                var root = doc.RootElement;

                string username = root.GetProperty("username").GetString() ?? "";
                string password = root.GetProperty("password").GetString() ?? "";
                string confirmPassword = root.GetProperty("confirmPassword").GetString() ?? "";

                // =========================
                // Create DTO
                // =========================
                var newUserDto = new CreateUserDTO
                {
                    Username = username,
                    Password = password,
                    Role = Domain.Enums.UserRole.Customer
                };

                // =========================
                // Validation in Application Layer
                // =========================
                string? validationError = await _userService.ValidateUserAsync(newUserDto, confirmPassword);
                if (!string.IsNullOrEmpty(validationError))
                {
                    await ShowNotificationAsync(validationError, false);
                    return;
                }

                // =========================
                // Create User
                // =========================
                await _userService.CreateAsync(newUserDto);

                await ShowNotificationAsync("Account created successfully ✔", true);

                // بعد النجاح ارجع للـ LoginForm
                await Task.Delay(1000);
                this.Close();
            }
            catch (Exception ex)
            {
                await ShowNotificationAsync($"Error: {ex.Message}", false);
            }
        }
    }
}