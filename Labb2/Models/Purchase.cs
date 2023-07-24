using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labb2.Models;

public partial class Purchase
{
    public int Id { get; set; }
    [Display(Name = "Покупець")]
    public int CustomerId { get; set; }
    [Display(Name = "Машина")]
    public int CarId { get; set; }
    
    [Display(Name = "Дата покупки")]
    public DateTime Date { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
