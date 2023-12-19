
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ASP_MVC_PR_1.Models
{
    [Table("tblMovies")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [MinLength(3, ErrorMessage = "Длина названия должна быть не менее трёх символов")]
        [MaxLength(100, ErrorMessage = "Длина названия должна быть не более ста символов")]
        [DisplayName("Название")]
        public string? MovieName { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [MinLength(3, ErrorMessage = "Длина поля должна быть не менее трёх символов")]
        [MaxLength(50, ErrorMessage = "Длина поля должна быть не более пятидесяти символов")]
        [DisplayName("Режисёр")]
        public string? Director { get; set; }

        [Required(ErrorMessage="Поле обязательно для заполнения!")]
        [MinLength(3, ErrorMessage = "Длина поля должна быть не менее трёх символов")]
        [MaxLength(50, ErrorMessage = "Длина поля должна быть не более пятидесяти символов")]
        [DisplayName("Жанр")]
        
        public string? Genre { get; set; }

        [DisplayName("Изображение")]
        public string? PosterPath { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [MinLength(3, ErrorMessage ="Длина описания должна быть не менее трёх символов")]
        [MaxLength(int.MaxValue)]
        [DisplayName("Описание")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения!")]
        [Range(1895, 2024)]
        [DisplayName("Дата выхода в прокат")]
        public int RealeaseDate { get; set; }
    }
}
