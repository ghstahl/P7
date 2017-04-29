/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import RiotControl from 'riotcontrol';
function MovieStore() {
    riot.observable(this) // Riot provides our event emitter.

    var self = this

    self.movies = []

    self.lastSearch = null;
    self.error = false;

    /**
     * Reset tag attributes to hide the errors and cleaning the results list
     */
    self.resetData = function() {
        self.movies = [];
        self.error = false;
    }
    
    self.onFetchResult = (data) =>{
        console.log('movies_search_fetch_result:',data);
        RiotControl.trigger('localstorage_set',{key:'moviesCache',data:data.Search});
        self.trigger('movies_changed', data.Search)
    }

    self.on('app-mount', function() {
        console.log('MovieStore app-mount');
        RiotControl.on('movies_search_fetch_result', self.onFetchResult);
    })
    self.on('app-unmount', function() {
        console.log('MovieStore app-unmount');
        RiotControl.off('movies_search_fetch_result', self.onFetchResult);
    })
    // Our store's event handlers / API.
    // This is where we would use AJAX calls to interface with the server.
    // Any number of views can emit actions/events without knowing the specifics of the back-end.
    // This store can easily be swapped for another, while the view components remain untouched.


    self.on('movies_search', function(value) {
        console.log('movies_search:',value);
        var url = 'http://www.omdbapi.com/?s=' + value.searchTerm;
        RiotControl.trigger('fetch',url,null,{name:'movies_search_fetch_result'},false);
    })

    self.on('movie_fetch_detail', function(value) {
        console.log('movie_fetch_detail:',value);
        var url = 'http://www.omdbapi.com/?i=' + value.imdbID
            +'&tomatoes=true'
            +'&plot=full';
        RiotControl.trigger('fetch',url,null,{name:'movie_detail'},false);
    })

    self.on('movies_clear', function() {
        RiotControl.trigger('localstorage_remove',{key:'moviesCache'});
        self.trigger('movies_changed', [])
    })


    // The store emits change events to any listening views, so that they may react and redraw themselves.

}

if (typeof(module) !== 'undefined') module.exports = MovieStore;



