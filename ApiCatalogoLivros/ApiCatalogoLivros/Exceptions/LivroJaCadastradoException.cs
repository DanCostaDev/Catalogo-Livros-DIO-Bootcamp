using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoLivros.Exceptions
{
    public class LivroJaCadastradoException : Exception
    {
        public LivroJaCadastradoException() : base("Este livro já está cadastrado.") {}
    }
}
