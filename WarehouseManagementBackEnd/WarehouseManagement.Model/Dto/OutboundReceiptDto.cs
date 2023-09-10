﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class OutboundReceiptDto:BaseDto
    {
        public string? To { get; set; }

        public Guid WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
    }
}