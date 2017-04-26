/**
 * Created by Herb on 9/27/2016.
 */

function LocalStorageStore() {
    riot.observable(this) // Riot provides our event emitter.
    var self = this

    /*
     {
         key:[string:required],
         data: [Object],
         trigger:[string:optional]
     }
     */

    self.on('localstorage_set', function(query) {
        console.log('localstorage_set:',query);
        localStorage.setItem(query.key, JSON.stringify(query.data));
        if(query.trigger){
            self.trigger(query.trigger) // in case you want an ack
        }
    })

    /*
    {
        key:'myKey',
        trigger:'myTrigger'
    }
     */
    self.on('localstorage_get', function(query) {
        console.log('localstorage_get:',query);
        var stored = localStorage.getItem(query.key);
        if(stored && stored != "undefined"){
            var restoredSession = JSON.parse(stored);
            self.trigger(query.trigger, restoredSession)
        }else{
            self.trigger(query.trigger, null)
        }
    })

    /*
     {
     key:'myKey',
     trigger:'myTrigger'
     }
     */
    self.on('localstorage_remove', function(query) {
       console.log('localstorage_get:',query);
       localStorage.removeItem(query.key);
    })

    /*

     */
    self.on('localstorage_clear', function() {
        console.log('localstorage_get:');
        localStorage.clear();
    })
}

if (typeof(module) !== 'undefined') module.exports = LocalStorageStore;



