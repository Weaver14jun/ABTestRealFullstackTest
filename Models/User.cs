using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackTest.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Некорректная дата регистрации")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Некорректная дата последней активности")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastActivityDate { get; set; }
    }
}
