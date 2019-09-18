using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Processo
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Uf { get; set; }
        public string Criacao { get; set; }
        public decimal Valor { get; set; }
        public string NumeroProcesso { get; set; }
        //public string Id_Cliente { get; set; }
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }  
        public virtual Cliente Cliente { get; set; }
    }
}