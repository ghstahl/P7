/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import 'whatwg-fetch';

function FetchStore() {
    var self = this
    self.name = 'FetchStore';
    self.namespace = self.name+':';
    riot.EVT.fetchStore ={
        in:{
            fetch:self.namespace +'fetch'
        },
        out:{
            inprogressDone:riot.EVT.progressStore.in.inprogressDone,
            inprogressStart:riot.EVT.progressStore.in.inprogressStart
        }
    }

    riot.observable(self) // Riot provides our event emitter.

    self.fetchException = null;
   
    /**
     * Reset tag attributes to hide the errors and cleaning the results list
     */
    self.resetData = function() {
        self.fetchException = null;
    }

    self.onRiotTrigger = (query,data)=>{
        if(query.query){
            riot.control.trigger(query.evt,query.query);
        }else{
            riot.control.trigger(query.evt);
        }
    }
    self.on(riot.EVT.app.out.appMount, function() {
        console.log(self.name,riot.EVT.app.out.appMount);
        riot.control.on('riot-trigger', self.onRiotTrigger);
    })

    self.on(riot.EVT.app.out.appUnmount, function() {
        console.log(self.name,'app-unmount');
        riot.control.off('riot-trigger', self.onRiotTrigger);
    })
    self.doRiotControlFetchRequest = function(input,init,trigger,jsonFixup ) {
        // we are a json shop

        riot.control.trigger(riot.EVT.fetchStore.out.inprogressStart);
        if(jsonFixup == true){
            if(!init){
                init = {}
            }

            if(!init.credentials){
                init.credentials = 'include'
            }

            if(!init.headers){
                init.headers = {}
            }

            if(!init.headers['Content-Type']){
                init.headers['Content-Type'] = 'application/json'
            }
            if(!init.headers['Accept']){
                init.headers['Accept'] = 'application/json'
            }

            if(init.body){
                var type = typeof(init.body)
                if(type === 'object'){
                    init.body = JSON.stringify(init.body)
                }
            }
        }

        let myTrigger = JSON.parse(JSON.stringify(trigger));

        fetch(input,init).then(function (response) {
            riot.control.trigger(riot.EVT.fetchStore.out.inprogressDone);
            var result = {response:response};
            if(response.status == 204){
                result.error = 'Fire the person that returns this 204 garbage!';
                riot.control.trigger(myTrigger.name,result,myTrigger);
            }
            if(response.ok){
                if(init.method == 'HEAD'){
                    riot.control.trigger(myTrigger.name,result,myTrigger);
                }else{
                    response.json().then((data)=>{
                        console.log(data);
                        result.json = data;
                        result.error = null;
                        riot.control.trigger(myTrigger.name,result,myTrigger);
                    });
                }
            }else{
                riot.control.trigger(myTrigger.name,result,myTrigger);
            }
        }).catch(function(ex) {
            console.log('fetch failed', ex)
            self.error = ex;
            //todo: event out error to mytrigger
            riot.control.trigger(riot.EVT.fetchStore.out.inprogressDone);
        });
    }

    // Our store's event handlers / API.
    // This is where we would use AJAX calls to interface with the server.
    // Any number of views can emit actions/events without knowing the specifics of the back-end.
    // This store can easily be swapped for another, while the view components remain untouched.

    self.on(riot.EVT.fetchStore.in.fetch, function(input,init,trigger,jsonFixup = true) {
        console.log(riot.EVT.fetchStore.in.fetch,input,init,trigger,jsonFixup);
        self.doRiotControlFetchRequest(input,init,trigger,jsonFixup);
    })

    // The store emits change events to any listening views, so that they may react and redraw themselves.

}

if (typeof(module) !== 'undefined') module.exports = FetchStore;




