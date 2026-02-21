using ECommerceManagement.Application.DTOs.UserDTOs;
using ECommerceManagement.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ECommerceManagement.Presentation.Forms.MainForms
{
    public partial class AdminMainForm : Form
    {
        private readonly IUserService _userService;
        private readonly UserDTO _currentUser;

        public AdminMainForm(IUserService userService, UserDTO user)
        {
            _userService = userService;
            _currentUser = user;

        }
    }
}
