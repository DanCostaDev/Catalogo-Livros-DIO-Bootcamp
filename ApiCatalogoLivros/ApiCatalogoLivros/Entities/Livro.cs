using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoLivros.Entities
{
    public class Livro
    {    
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Editora { get; set; }
        public double Preco { get; set; }

    }
}

