using KD25_BitirmeProjesi.CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.CoreLayer.Abstracts
{
	public interface IBaseEntity
	{
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }// = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public RecordStatus RecordStatus { get; set; } //= false;
    }
}
