using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CRUDAjaxTable.Models
{
    public class Operation
    {
        public int Id { get; set; }
        [Required]
        public double Cost { get; set; }
        public string Description { get; set; }
        [Required]
        // converter enum
        [JsonConverter(typeof(StringEnumConverter))]
        public TypeOperation TypeOperation { get; set; }
        [Required]
        public virtual Author Author { get; set; }
        [NotMapped]
        public Array AllOperations { get; set; }
    }
}