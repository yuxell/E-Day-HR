using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Enums;

namespace KD25_BitirmeProjesi.CoreLayer.Interfaces
{
    public interface IEntityRequest
    {
        DateTime RequestDate { get; set; }
        ApprovalStatus ApprovalStatus { get; set; }
        DateTime? ResponseDate { get; set; }

    }
}
