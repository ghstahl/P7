using System;
using P7.BlogStore.Core;
using P7.Core.Utils;

namespace P7.BlogStore.Hugo
{
    public class BlogCommentRecord: DocumentBase
    {
        public Guid BlogId_G
        {
            get
            {
                if (string.IsNullOrEmpty(BlogId))
                {
                    return Guid.Empty;
                }
                return Guid.Parse(BlogId);
            }
        }

        public string BlogId { get; set; }
        public BlogComment BlogComment { get; set; }

        public BlogCommentRecord()
        {
        }

        public BlogCommentRecord(BlogCommentRecord doc)
        {
            this.BlogId = doc.BlogId;
            this.BlogComment = doc.BlogComment;
        }
        public BlogCommentRecord(string blogId,BlogComment blogComment)
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

        private string _id = Guid.Empty.ToString();
        private Guid _id_G = Guid.Empty;
        public string Id
        {
            get
            {
                if (_id_G == Guid.Empty &&  BlogComment != null)
                {
                    _id_G = GuidGenerator.CreateGuid(BlogId_G, BlogComment.Id);
                    _id = _id_G.ToString();
                }
                return _id;
            }
        }
    }
}