﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVC.Models
{
    public class Tarea
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool Completa { get; set; } = false;

        [ForeignKey("Tablero")]
        public int tableroId { get; set; }

    }
}
