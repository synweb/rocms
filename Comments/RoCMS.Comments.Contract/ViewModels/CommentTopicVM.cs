using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Comments.Contract.Models;

namespace RoCMS.Comments.Contract.ViewModels
{
    public class CommentTopicVM: CommentTopic
    {
        public int CommentCount { get; set; }
    }
}
