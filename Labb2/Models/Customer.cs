using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Labb2.Models;

public partial class Customer
{
    private const string ER1= "Поле необхідно заповнити";
    private const string email= @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
    public int Id { get; set; }
    [Required(ErrorMessage =ER1)]
    [Display(Name="Ім\'я")]
    [RegularExpression(@"^[А-Я]+[а-яА-Я\s]*$", ErrorMessage = "У цьому полі ви можете вводити лише букви, введіть коректно")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = ER1)]
    [Display(Name = "Прізвище")]
    [RegularExpression(@"^[А-Я]+[а-яА-Я\s]*$", ErrorMessage = "У цьому полі ви можете вводити лише букви, введіть коректно")]
    public string Surname { get; set; } = null!;
    [Required(ErrorMessage = ER1)]
    [Display(Name = "Електронна пошта")]
    [RegularExpression(email,ErrorMessage ="Введіть правильну електрону адресу")]
    public string Email { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
