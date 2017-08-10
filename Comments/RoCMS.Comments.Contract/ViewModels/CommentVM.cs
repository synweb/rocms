using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Comments.Contract.Models;

namespace RoCMS.Comments.Contract.ViewModels
{
    public class CommentVM:Comment
    {
        public CommentVM()
        {
            Replies = new List<CommentVM>();
        }

        public ICollection<CommentVM> Replies { get; set; }
        public string Author { get; set; }


    }
}
