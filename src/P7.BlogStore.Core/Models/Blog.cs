using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace P7.BlogStore.Core
{
    public class Blog: DocumentBase
    {
        [JsonIgnore]
        public bool EnableDeepCompare { get; set; }

        public BlogMetaData MetaData { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public string Data { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public Blog()
        {
            EnableDeepCompare = false;
        }

        public Blog(Blog doc)
        {
            this.Id = doc.Id;
            this.Categories = doc.Categories;
            this.Data = doc.Data;
            this.MetaData = doc.MetaData;
            this.Tags = doc.Tags;
            this.TimeStamp = doc.TimeStamp;
            this.Summary = doc.Summary;
            this.Title = doc.Title;
        }
       
        public override bool Equals(object obj)
        {
            return EnableDeepCompare ? DeepEquals(obj) : ShallowEquals(obj);
        }
        public bool ShallowEquals(object obj)
        {
            var other = obj as Blog;
            if (other == null)
            {
                return false;
            }

            if (!Id.IsEqual(other.Id))
            {
                return false;
            }
            return true;
        }
        public bool DeepEquals(object obj)
        {
            var other = obj as Blog;
            if (other == null)
            {
                return false;
            }

            var bothNull = Categories == null && other.Categories == null;
            var bothNotNull = Categories != null && other.Categories != null;

            if (bothNotNull)
            {
                if (Categories.Except(other.Categories).Any())
                    return false;
            }
            else if (!bothNull)
            {
                return false;
            }

            bothNull = Tags == null && other.Tags == null;
            bothNotNull = Tags != null && other.Tags != null;
            if (bothNotNull)
            {
                if (Tags.Except(other.Tags).Any())
                    return false;
            }
            else if (!bothNull)
            {
                return false;
            }

            if (!Id.IsEqual(other.Id))
            {
                return false;
            }
            if (!Data.IsEqual(other.Data))
            {
                return false;
            }
            if (!MetaData.IsEqual(other.MetaData))
            {
                return false;
            }
            if (!TimeStamp.IsEqual(other.TimeStamp))
            {
                return false;
            }
            if (!Summary.IsEqual(other.Summary))
            {
                return false;
            }
            if (!Title.IsEqual(other.Title))
            {
                return false;
            }
            return true;

        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}