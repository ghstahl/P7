using System;

namespace P7.BlogStore.Core
{
    public class BlogComment : IDocumentBase
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }

        public BlogComment()
        {
        }

        public BlogComment(BlogComment doc)
        {
            this.Id = doc.Id;
            this.Comment = doc.Comment;
        }
        public override bool Equals(object obj)
        {
            var other = obj as BlogComment;
            if (other == null)
            {
                return false;
            }

            if (!Id.IsEqual(other.Id))
            {
                return false;
            }
            if (!Comment.IsEqual(other.Comment))
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