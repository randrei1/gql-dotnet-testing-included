using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
