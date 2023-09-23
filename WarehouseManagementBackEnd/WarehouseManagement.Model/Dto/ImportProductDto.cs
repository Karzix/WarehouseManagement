﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class ImportProductDto :BaseDto
    {
        public int Quantity { get; set; }

        public Guid SupplierProductId { get; set; }
        public string? SipplierName { get; set; }
        public string? ProductName { get; set; }

        public Guid InboundReceiptId { get; set; }
    }
}