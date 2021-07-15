using System;
using System.Collections.Generic;

namespace TSGSystemsToolkit.DesktopUI.Library.Models
{
    public interface ILoggedInUserModel
    {
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string Id { get; set; }
        string Roles { get; set; }
        string Token { get; set; }

        void ResetUserModel();
    }
}