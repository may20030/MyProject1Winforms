using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Application.DTOs.UserDTOs;
using System.Threading.Tasks;
using ECommerceManagement.Presentation.Forms.MainForms;

namespace ECommerceManagement.Presentation.Forms.LoginForms
{
    public class LoginForm : Form
    {

        // خليها كده
        private readonly IUserService _userService;
        private readonly ICartItemService _cartService;
        private WebView2 webView;

        // Notification Panel
        private Panel notificationPanel;
        private Label notificationLabel;

        private Button btnSignUp;

        private void InitializeSignUpButton()
        {
            btnSignUp = new Button
            {
                Text = "Create Account",
                Width = 150,
                Height = 40,
                Top = 10,
                Left = this.ClientSize.Width - 170 // على اليمين
            };
            btnSignUp.Click += BtnSignUp_Click;

            this.Controls.Add(btnSignUp);
        }
        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            var signUpForm = new SignUpForm(_userService);
            signUpForm.ShowDialog(); // يظهر الفورم فوق LoginForm
        }
        public LoginForm(IUserService userService, ICartItemService cartService)
        {
            _userService = userService;
            _cartService = cartService;
            InitializeComponent();
            InitializeNotificationPanel();
            InitializeSignUpButton();
        }

        private  void InitializeComponent()
        {
            this.Text = "Login";
            this.Width = 800;
            this.Height = 800;

            webView = new WebView2();
            webView.Dock = DockStyle.Fill;
            this.Controls.Add(webView);

            this.Load += LoginForm_Load; // مهم جدًا
        }
        private async void LoginForm_Load(object sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async();

            string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "UI", "login.html");
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
                Width = 350,
                Height = 80,
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

        // =========================
        // Show Notification
        // =========================
        private async Task ShowNotificationAsync(string message, bool isSuccess)
        {
            if (this.InvokeRequired)
            {
                await this.Invoke(new Func<Task>(() => ShowNotificationAsync(message, isSuccess)));
                return;
            }

            notificationLabel.Text = message;
            notificationPanel.BackColor = isSuccess
                ? System.Drawing.Color.FromArgb(76, 175, 80)   // أخضر
                : System.Drawing.Color.FromArgb(244, 67, 54);  // أحمر

            notificationPanel.BringToFront();
            notificationPanel.Visible = true;

            await Task.Delay(2000);

            notificationPanel.Visible = false;
        }

        // =========================
        // Open MainForm
        // =========================
        private void OpenMainForm(UserDTO user)
        {
            this.Hide();

            Form mainForm;

            if (user.Role == Domain.Enums.UserRole.Admin)
            {
                mainForm = new AdminMainForm(_userService, user);
            }
            else
            {
                mainForm = new CustomerMainForm(_userService, _cartService, user);
            }

            // اجعل الفورم يظهر في المقدمة
            mainForm.TopMost = true;   // يخلي الفورم فوق كل النوافذ
            mainForm.Show();
            mainForm.Activate();       // يعطيه التركيز
            mainForm.TopMost = false;  // لو عايز الفورم يتصرف طبيعي بعد الظهور

            // لما الـ MainForm يقفل نرجع الـ LoginForm للظهور
            mainForm.FormClosed += (s, e) =>
            {
                this.Close();
            };
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

                string action = root.GetProperty("action").GetString();
                if (action == "signup")
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        var signUpForm = new SignUpForm(_userService);
                        signUpForm.ShowDialog();
                    }));
                    return;
                }
                if (action == "login")
                {
                    string username = root.GetProperty("username").GetString() ?? "";
                    string password = root.GetProperty("password").GetString() ?? "";

                    var user = await _userService.LoginAsync(new LoginDTO
                    {
                        Username = username,
                        Password = password
                    });

                    if (user != null)
                    {
                        await ShowNotificationAsync($"Welcome {user.Username}! ✔", true);

                        // نضيف Delay صغير قبل فتح MainForm عشان يظهر الترحيب
                        await Task.Delay(500);

                        OpenMainForm(user);
                    }
                    else
                    {
                        await ShowNotificationAsync("Invalid username or password ❌", false);
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowNotificationAsync($"Error processing login: {ex.Message}", false);
            }
        }
    }
}