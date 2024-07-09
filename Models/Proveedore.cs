using System;
using System.Collections.Generic;

namespace PruebaUnitaria.Models;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string Codigo { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string Rfc { get; set; } = null!;
}
