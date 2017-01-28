using System;
using System.Linq;

namespace P7.BlogStore.Core
{
    public interface IDocumentBase
    {
        Guid Id { get;  }
    }
    public class Blog: IDocumentBase
    {
        public BlogMetaData MetaData { get; set; }
        public string[] Categories { get; set; }
        public string[] Tags { get; set; }
        public string Data { get; set; }

        public Blog()
        {
        }

        public Blog(Blog doc)
        {
            this.Id = doc.Id;
            this.Categories = doc.Categories;
            this.Data = doc.Data;
            this.MetaData = doc.MetaData;
            this.Tags = doc.Tags;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Blog;
            if (other == null)
            {
                return false;
            }

            if (Categories != null && other.Categories != null)
            {
                if (Categories.Except(other.Categories).Any())
                    return false;
            }
            if (!(Categories == null && other.Categories == null))
            {
                return false;
            }
            if (Tags != null && other.Tags != null)
            {
                if (Tags.Except(other.Tags).Any())
                    return false;
            }
            if (!(Tags == null && other.Tags == null))
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
            return true;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public Guid Id { get; set; }
    }
}