using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RadioCab.Models;

[Index("Email", Name = "UQ__Users__A9D10534E5EA3B98", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserID { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(150)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(20)]
    public string? Role { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
