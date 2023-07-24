using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb2.Models;

public partial class Producer
{
    private const string ER1 = "Поле необхідно заповнити";
    public int Id { get; set; }
    
    [Display(Name = "Країна")]
    public int CountryId { get; set; }
    [Required(ErrorMessage = ER1)]
    [Display(Name = "Назва виробника")]
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "У цьому полі ви можете вводити лише букви, введіть коректно")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    
    [Display(Name = "Країна")]
    public virtual Country Country { get; set; } = null!;
}
