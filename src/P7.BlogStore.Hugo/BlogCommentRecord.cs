using System;
using P7.BlogStore.Core;
using P7.Core.Utils;

namespace P7.BlogStore.Hugo
{
    public class BlogCommentRecord: IDocumentBase
    {
        public Guid BlogId { get; set; }
        public BlogComment BlogComment { get; set; }

        public BlogCommentRecord()
        {
        }

        public BlogCommentRecord(BlogCommentRecord doc)
        {
            this.BlogId = doc.BlogId;
            this.BlogComment = doc.BlogComment;
        }
        public BlogCommentRecord(Guid blogId,BlogComment blogComment)
        {
            this.BlogId = blogId;
            this.BlogComment = blogComment;
        }
        public override bool Equals(object obj)
        {
            var other = obj as BlogCommentRecord;
            if (other == null)
            {
                return false;
            }

            if (!BlogId.IsEqual(other.BlogId))
            {
                return false;
            }
            if (!BlogComment.IsEqual(other.BlogComment))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return BlogComment.Id.GetHashCode() ^ BlogId.GetHashCode();
        }
        private Guid _id = Guid.Empty;

        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty && BlogComment != null)
                {
                    _id = GuidGenerator.CreateGuid(BlogId, BlogComment.Id.ToString());
                }
                return _id;
            }
        }
    }
}