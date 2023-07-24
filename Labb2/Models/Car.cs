using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb2.Models;

public partial class Car
{
    private const string ER1 = "Поле необхідно заповнити";
    public int Id { get; set; }
    
    [Display(Name = "Виробник")]
    
    public int ProducerId { get; set; }
    
    [Required(ErrorMessage = ER1)]
    [Display(Name = "Назва машини")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = ER1)]
    [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "Введіть коректну вартість")]
    [Range(1000, int.MaxValue,ErrorMessage ="Ціна повинна бути більше 10000")]
    [Display(Name = "Ціна машини")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = ER1)]
    [Display(Name = "Короткий опис")]
    public string Description { get; set; } = null!;
    
    [Display(Name = "Виробник")]
    public virtual Producer Producer { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
