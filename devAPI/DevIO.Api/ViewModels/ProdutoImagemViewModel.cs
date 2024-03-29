﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.ViewModels
{
    public class ProdutoImagemViewModel
    {
        public Guid Id { get; set; }
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }
        
        public IFormFile ImagemUpload { get; set; }
        
        public string Imagem { get; set; }
        
        public decimal Valor { get; set; }
        
        public DateTime DataCadastro { get; set; }
        
        public bool Ativo { get; set; }
        
        [ScaffoldColumn(false)]
        public string NomeFornecedor { get; set; }
    }
}
