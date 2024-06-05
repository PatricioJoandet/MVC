using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Tablero
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Titulo { get; set;}
        public string Subtitulo { get; set; }
        public string Color { get; set;}
    }
}
