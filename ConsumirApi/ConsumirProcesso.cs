using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumirApi
{
    public class ConsumirProcesso
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Uf { get; set; }
        public string Criacao { get; set; }
        public decimal Valor { get; set; }
        public string NumeroProcesso { get; set; }
        public int ClienteID { get; set; }
    }
}
