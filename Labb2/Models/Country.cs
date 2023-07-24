using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb2.Models;

public partial class Country
{
    public int Id { get; set; }
    [Required(ErrorMessage ="Поле необхідно заповнити")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Producer> Producers { get; set; } = new List<Producer>();
}
