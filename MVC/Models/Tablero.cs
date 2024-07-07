using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Tablero
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set;}
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Subtitulo { get; set; }
        public string Color { get; set;}

        [ForeignKey("Usuario")]
        public int userId { get; set; }

        //public Usuario Usuario { get; set; }
    }
}
