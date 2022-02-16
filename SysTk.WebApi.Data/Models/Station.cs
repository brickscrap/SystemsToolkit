using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.WebApi.Data.Models
{
    public class Station : BaseEntity
    {
        private string _id;

        [Key]
        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        public string Id
        {
            get { return _id.ToUpper(); }
            set { _id = value.ToUpper(); }
        }


        [Required]
        public Cluster Cluster { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        [Column(TypeName = "varchar(15)")]
        public string IP { get; set; }

        public List<FtpCredentials> FtpCredentials { get; set; }
    }

    public enum Cluster
    {
        ShellRBA,
        ShellRFA,
        Shell,
        Esso,
        Independents,
        Rontec,
        BP,
        Certas,
        Texaco
    }
}
