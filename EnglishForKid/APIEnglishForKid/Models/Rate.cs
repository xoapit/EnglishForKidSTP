﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIEnglishForKid.Models
{
    public class Rate : BaseDataObject
    {
        public int Level { get; set; }
        public string ApplicationUserID { get; set; }
        public Guid LessonID { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Lesson Lesson { get; set; }

        public Rate()
        {
            CreateAt = DateTime.Now;
        }
    }
}