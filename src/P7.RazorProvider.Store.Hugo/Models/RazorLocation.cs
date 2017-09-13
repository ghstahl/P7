using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using P7.Core.Utils;

namespace P7.RazorProvider.Store.Hugo.Models
{

    public class RazorLocation : IComparable
    {
        public string Location { get; set; }
        public string Id => Location;
        public string Content { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime LastRequested { get; set; }

      
        public RazorLocation()
        {
        }

        public RazorLocation(RazorLocation doc)
        {
            
            this.Location = doc.Location;
            this.Content = doc.Content;
            this.LastModified = doc.LastModified;
            this.LastRequested = doc.LastRequested;
        }
       
        public override bool Equals(object obj)
        {
            return  ShallowEquals(obj);
        }
        public bool ShallowEquals(object obj)
        {
            var other = obj as RazorLocation;
            if (other == null)
            {
                return false;
            }

            return true;
        }

        public int CompareTo(object obj)
        {
            if (Equals(obj))
                return 0;
            return -1;
        }
    }
}