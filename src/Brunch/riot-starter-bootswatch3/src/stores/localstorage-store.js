/**
 * Created by Herb on 9/27/2016.
 */

function LocalStorageStore() {
    var self = this
    self.name = 'LocalStorageStore';
    riot.EVT.localStorageStore ={
        in:{
            localstorageSet:'localstorage-set',
            localstorageGet:'localstorage-get',
            localstorageRemove:'localstorage-remove',
            localstorageClear:'localstorage-clear'
        },
        out:{}
    }

    riot.observable(self) // Riot provides our event emitter.


    /*
     {
         key:[string:required],
         data: [Object],
         trigger:[optional]{
                event:[string],
                riotControl:bool  // do a riotcontrol.trigger or just an observable trigger.
         }
     }
     */

    self.on(riot.EVT.localStorageStore.in.localstorageSet, function(query) {
        console.log(riot.EVT.localStorageStore.in.localstorageSet,query);
        localStorage.setItem(query.key, JSON.stringify(query.data));
        if(query.trigger){
            self.trigger(query.trigger) // in case you want an ack
        }
    })

    /*
    {
        key:'myKey',
        trigger:{
                event:[string],
                riotControl:bool  // do a riotcontrol.trigger or just an observable trigger.
         }
    }
     */
    self.on(riot.EVT.localStorageStore.in.localstorageGet, function(query) {
        console.log(riot.EVT.localStorageStore.in.localstorageGet,query);
        var stored = localStorage.getItem(query.key);
        var data = null;
        if(stored && stored != "undefined"){
            data = JSON.parse(stored);
        }
        if(query.trigger.riotControl == true){
            riot.control.trigger(query.trigger.event,data);
        }else{
            self.trigger(query.trigger.event, data);
        }
    })

    /*
     {
     key:'myKey' 
     }
     */
    self.on(riot.EVT.localStorageStore.in.localstorageRemove, function(query) {
       console.log(riot.EVT.localStorageStore.in.localstorageRemove,query);
       localStorage.removeItem(query.key);
    })

    /*

     */
    self.on(riot.EVT.localStorageStore.in.localstorageClear, function() {
        console.log(riot.EVT.localStorageStore.in.localstorageClear);
        localStorage.clear();
    })
}

if (typeof(module) !== 'undefined') module.exports = LocalStorageStore;



