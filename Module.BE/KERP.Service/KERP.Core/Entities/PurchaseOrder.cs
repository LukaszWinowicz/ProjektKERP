using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KERP.Core.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public required string PurchaseOrderNumber { get; set; }
        public required int LineNumber { get; set; }
        public required int Sequence { get; set; }
    }
}
