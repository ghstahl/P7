using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using P7.RazorProvider.Store.Hugo.Interfaces;
using P7.RazorProvider.Store.Hugo.Models;
using P7.SimpleDocument.Store;

namespace P7.Razor.FileProvider
{
    public class DatabaseFileProvider : IFileProvider
    {
        private IRazorLocationStore _store;

        public DatabaseFileProvider(IRazorLocationStore store)
        {
            _store = store;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var result = new DatabaseFileInfo(_store, subpath);
            return result.Exists ? result as IFileInfo : new NotFoundFileInfo(subpath); 
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch(string filter)
        {
            return new DatabaseChangeToken(_store, filter);
        }
    }
    internal class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Instance { get; } = new EmptyDisposable();
        private EmptyDisposable() { }
        public void Dispose() { }
    }
    public class DatabaseChangeToken : IChangeToken
    {
        private string _viewPath;
        private IRazorLocationStore _store;
        public DatabaseChangeToken(IRazorLocationStore store, string viewPath)
        {
            _store = store;
            _viewPath = viewPath;
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => EmptyDisposable.Instance;


        public bool HasChanged
        {
            get
            {
                var query = new SimpleDocument<RazorLocation>()
                {
                    Document = new RazorLocation()
                    {
                        Location = _viewPath
                    }
                };

                var doc = _store.FetchAsync(query.Id_G).GetAwaiter().GetResult();
                if (doc != null)
                {
                    return doc.Document.LastModified > doc.Document.LastRequested;
                }
                return false;
            }
        }

        public bool ActiveChangeCallbacks => false;
    }

    public class DatabaseFileInfo : IFileInfo
    {
        private IRazorLocationStore _store;
        private string _viewPath;
        private byte[] _viewContent;
        private DateTimeOffset _lastModified;
        private bool _exists;
        public DatabaseFileInfo(IRazorLocationStore store, string viewPath)
        {
            _viewPath = viewPath;
            _store = store;
        }

        public Stream CreateReadStream()
        {
            return new MemoryStream(_viewContent);
        }

        public bool Exists => _exists;
        public long Length
        {
            get
            {
                using (var stream = new MemoryStream(_viewContent))
                {
                    return stream.Length;
                }
            }
        }

        public async Task GetView()
        {
            var query = new SimpleDocument<RazorLocation>()
            {
                Document = new RazorLocation()
                {
                    Location = _viewPath
                }
            };

            var doc = await _store.FetchAsync(query.Id_G);
            if (doc != null)
            {
                _viewContent = Encoding.UTF8.GetBytes(doc.Document.Content);
                _lastModified = doc.Document.LastModified;
                doc.Document.LastRequested = DateTime.UtcNow;
                await _store.UpdateAsync(doc);
            }
            
        }
    

        public string Name => Path.GetFileName(_viewPath);

        public string PhysicalPath => null;
        public DateTimeOffset LastModified => _lastModified;
        public bool IsDirectory => false;
    }
}
