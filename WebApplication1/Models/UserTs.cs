using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserTs
    { 
        // id
        [Key] 
        public string id { get; set; }
        // 姓名
        public string cusName { get; set; }
    }
}