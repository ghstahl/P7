
class RiotControlStore{

  constructor(){
    var self = this;
    self.name = 'RiotControlStore';
    self.namespace = self.name + ':';
    riot.EVT.riotControlStore ={
        in:{
          riotContolAddStore:self.namespace+'riot-contol-add-store',
          riotContolRemoveStore:self.namespace+'riot-contol-remove-store'
        },
        out:{}
    }
    riot.observable(this);
    this.bindEvents();
    this._stores = {};
   
  }

  bindEvents(){
    this.on(riot.EVT.riotControlStore.in.riotContolAddStore, (name,store) => {
      var tempStore = riot.control._stores;
      this._stores[name] = store;
      console.log(riot.EVT.riotControlStore.in.riotContolAddStore,store)
      riot.control.addStore(store)
    });

    this.on(riot.EVT.riotControlStore.in.riotContolRemoveStore, (name) => {
      console.log(riot.EVT.riotControlStore.in.riotContolRemoveStore,name)
      var store = this._stores[name];
      while (riot.control._stores.indexOf(store) !== -1) {
        riot.control._stores.splice(riot.control._stores.indexOf(store), 1);
      }
    });
  }
}
export default RiotControlStore;