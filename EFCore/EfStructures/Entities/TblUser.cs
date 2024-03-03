using System;
using System.Collections.Generic;

namespace EFCore.EfStructures.Entities;

public partial class TblUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
