using ECommerceManagement.Application.DTOs.CartItemDTOs;
using ECommerceManagement.Application.DTOs.UserDTOs;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Presentation.Forms.LoginForms;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerceManagement.Presentation.Forms.MainForms
{
    public class CustomerMainForm : Form
    {
        private readonly IUserService _userService;
        private readonly ICartItemService _cartService;
        private readonly UserDTO _currentUser;
        private WebView2 webView;

        public CustomerMainForm(IUserService userService, ICartItemService cartService, UserDTO user)
        {
            _userService = userService;
            _cartService = cartService;
            _currentUser = user;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = $"E-Commerce Dashboard - {_currentUser.Username}";
            this.Width = 1400;
            this.Height = 900;

            webView = new WebView2
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(webView);

            this.Load += CustomerMainForm_Load;
        }

        private async void CustomerMainForm_Load(object sender, EventArgs e)
        {
            await webView.EnsureCoreWebView2Async();

            string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "UI", "mainform.html");
            if (!File.Exists(path))
            {
                MessageBox.Show($"File not found: {path}");
                return;
            }

            webView.Source = new Uri(path);
            webView.CoreWebView2.WebMessageReceived += WebMessageReceived;

            // Set username dynamically in HTML
            webView.CoreWebView2.PostWebMessageAsJson($"{{\"action\":\"setUsername\",\"name\":\"{_currentUser.Username}\"}}");
        }

        private async void WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(e.WebMessageAsJson);
                var root = doc.RootElement;

                if (!root.TryGetProperty("action", out var actionProp)) return;
                string action = actionProp.GetString() ?? "";

                switch (action)
                {
                    case "navigate":
                        if (root.TryGetProperty("page", out var pageProp))
                        {
                            string page = pageProp.GetString() ?? "mainform.html";
                            string pagePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "UI", page);
                            if (File.Exists(pagePath))
                                webView.Source = new Uri(pagePath);
                            else
                                await ShowNotificationAsync($"Page not found: {page}", false);
                        }
                        break;

                    case "addToCart":
                        if (root.TryGetProperty("productId", out var prodIdProp))
                        {
                            int productId = prodIdProp.GetInt32();
                            int quantity = 1;
                            if (root.TryGetProperty("quantity", out var qtyProp))
                                quantity = qtyProp.GetInt32();

                            var dto = new CreateCartItemDTO
                            {
                                UserId = _currentUser.Id,
                                ProductId = productId,
                                Quantity = quantity
                            };

                            await _cartService.AddToCartAsync(dto);
                            await ShowNotificationAsync("Item added to cart ✔", true);
                        }
                        break;

                    case "updateCartItem":
                        if (root.TryGetProperty("productId", out var upProdIdProp) &&
                            root.TryGetProperty("quantity", out var upQtyProp))
                        {
                            int productId = upProdIdProp.GetInt32();
                            int quantity = upQtyProp.GetInt32();

                            // Get all cart items to find the Id
                            var cartItems = await _cartService.GetAllAsync(_currentUser.Id);
                            var cartItem = cartItems.Find(c => c.ProductId == productId);
                            if (cartItem != null)
                            {
                                await _cartService.UpdateAsync(new UpdateCartItemDTO
                                {
                                    Id = cartItem.Id,
                                    Quantity = quantity
                                });

                                await ShowNotificationAsync("Cart updated ✔", true);
                            }
                        }
                        break;

                    case "logout":
                        this.Hide();
                        var loginForm = new LoginForm(_userService, _cartService); // ✅ صح
                        loginForm.Show();
                        this.Close();
                        break;

                    default:
                        await ShowNotificationAsync($"Unknown action: {action}", false);
                        break;
                }
            }
            catch (Exception ex)
            {
                await ShowNotificationAsync($"Error processing message: {ex.Message}", false);
            }
        }

        // =========================
        // Notifications (to HTML)
        // =========================
        private async Task ShowNotificationAsync(string message, bool isSuccess)
        {
            if (webView?.CoreWebView2 != null)
            {
                string type = isSuccess ? "success" : "error";
                string json = $"{{\"action\":\"showNotification\",\"message\":\"{message}\",\"type\":\"{type}\"}}";
                webView.CoreWebView2.PostWebMessageAsJson(json);
            }

            await Task.CompletedTask;
        }
    }
}