using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $rootnamespace$.Models
{
    public class ViewProfileViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        public string Name { get; set; }

        [DisplayName("Email address")]
        public string EmailAddress { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MemberType { get; set; }

        public string Description { get; set; }

        public string Twitter { get; set; }

        public string LinkedIn { get; set; }

        public string Skype { get; set; }

        public DateTime LastLoginDate { get; set; }

        public int NumberOfLogins { get; set; }

        public int NumberOfProfileViews { get; set; }
    }
}