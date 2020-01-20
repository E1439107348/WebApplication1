using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class code_value
    {
        [Key]
        public int CODE_VALUE_SEQ { get; set; }
        public string CODE_TYPE { get; set; }
        public string CODE_COLUMN_NAME { get; set; }
        public string CODE_LEVEL { get; set; }
        public string CODE_VALUE { get; set; }
        public decimal ININO { get; set; }
        public string SUB_CODE_VALUE { get; set; }
        public string CODE_NAME { get; set; }
        public string CODE_NAME2 { get; set; }
        public string CODE_NAME3 { get; set; }
        public string CODE_REMARK { get; set; }
        public string CODE_SPELLING { get; set; }
        public string ISCUSTOMIZE { get; set; }
        public string CODE_ASSIST { get; set; }
        public string CODE_STATUS { get; set; }
        public string CODE_LEAF { get; set; }
        public string START_DATE { get; set; }
        public string STOP_DATE { get; set; }


    }
}