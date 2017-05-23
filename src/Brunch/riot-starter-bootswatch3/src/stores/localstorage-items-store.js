/**
 * Created by Herb on 9/27/2016.
 */
// TodoStore definition.
// Flux stores house application logic and state that relate to a specific domain.
// In this case, a list of todo items.
import RiotControl from 'riotcontrol';

function LocalStorageItemsStore(name) {
    var self = this

    riot.observable(self) // Riot provides our event emitter.

    self.items = null;

    self._localStorageKey = name + '-LS-key';
    self._localStorageGetEVT = name + '-LS-get';
    self._itemsChangedEVT = name + '-items-changed';
    self._getItemsEVT = name + '-items-get';
    self._clearItemsEVT = name + '-items-clear';
    self._addItemEVT = name + '-add';
    self._removeItemEVT = name + '-remove';

    self.onLocalStorageGet = (items) => {
        if(!items){
            self.items = []
            RiotControl.trigger(riot.EVT.localStorageStore.in.localstorageSet,{key:self._localStorageKey,data:self.items});
        }else{
            self.items = items;
        }
        self.trigger(self._itemsChangedEVT, self.items)
    }

    self.on(riot.EVT.app.out.appMount, function() {
        console.log('ClientCredentialStore',riot.EVT.app.out.appMount);
        RiotControl.on(self._localStorageGetEVT, self.onLocalStorageGet);
        self._asyncFetchItemFromStorage();
    })

    self.on(riot.EVT.app.out.appUnmount, function() {
        console.log('ClientCredentialStore',riot.EVT.app.out.appUnmount);
        RiotControl.off(self._localStorageGetEVT, self.onLocalStorageGet);
    })

    self._asyncFetchItemFromStorage = () => {
        console.log('handle:',self._getItemsEVT);
        if(self.items){
            self.trigger(self._itemsChangedEVT, self.items)
        }else{
            RiotControl.trigger(riot.EVT.localStorageStore.in.localstorageGet,{
                key:self._localStorageKey,
                trigger:self._localStorageGetEVT
            });
        }
    }

    self.on(self._getItemsEVT, self._asyncFetchItemFromStorage)

    self.on(self._clearItemsEVT, function() {
        console.log('handle:',self._clearItemsEVT);
        self.items = [];
        RiotControl.trigger(riot.EVT.localStorageStore.in.localstorageRemove,{key:self._localStorageKey});
        self.trigger(self._itemsChangedEVT, self.items)
    })

    self.on(self._addItemEVT, function(value,containsFunc) {
        console.log('handle:',self._addItemEVT,value,containsFunc);
        if(!self.items){
            self.items = [];
        }
        var addIt = true;
        if(containsFunc){
            addIt = !containsFunc(self.items,value);
        }
        if(addIt){
            self.items.push(value);
            RiotControl.trigger(riot.EVT.localStorageStore.in.localstorageSet,{key:self._localStorageKey,data:self.items});
            self.trigger(self._itemsChangedEVT, self.items)
        }
    })

    self.on(self._removeItemEVT, function(item,removeFunc) {
        console.log('handle:',self._removeItemEVT,item,removeFunc);
        if(removeFunc){
            self.items = removeFunc(self.items,item)
            RiotControl.trigger(riot.EVT.localStorageStore.in.localstorageSet,{key:self._localStorageKey,data:self.items});
            self.trigger(self._itemsChangedEVT, self.items)
        }
    })
}

if (typeof(module) !== 'undefined') module.exports = LocalStorageItemsStore;
